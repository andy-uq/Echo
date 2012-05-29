using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using Echo.Structures;
using Echo;

namespace Echo.Celestial
{
	partial class SolarSystem
	{
		public static class Builder
		{
			public static SolarSystemState Save(SolarSystem solarSystem)
			{
				return new SolarSystemState
				{
					Id = solarSystem.Id,
					Name = solarSystem.Name,
					LocalCoordinates = solarSystem.Position.LocalCoordinates,
					Satellites = solarSystem.Satellites.Save(),
					Structures = solarSystem.Structures.Save(),
					Ships = solarSystem.Ships.Save(),
				};
			}

			public static SolarSystem Build(StarCluster starCluster, SolarSystemState state)
			{
				var solarSystem = new SolarSystem
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(starCluster, state.LocalCoordinates),
				};

				solarSystem.Ships = (state.Ships ?? Enumerable.Empty<ShipState>())
					.Select(x => Echo.Ships.Ship.Builder.Build(solarSystem, x))
					.ToList();

				solarSystem.Satellites = (state.Satellites ?? Enumerable.Empty<CelestialObjectState>())
					.Select(x => CelestialObject.Builder.For(x).Build(solarSystem, x))
					.ToList();

				solarSystem.Structures = (state.Structures ?? Enumerable.Empty<StructureState>())
					.Select(x => Structure.Builder.For(x).Build(solarSystem, x))
					.ToList();

				var satellites = solarSystem.Satellites.ToDictionary(x => x.Id);
				var structures = solarSystem.Structures.ToDictionary(x => x.Id);

				AssignSatellites(state, satellites);
				AssignStructures(state, satellites, structures);

				return solarSystem;
			}

			private static void AssignStructures(SolarSystemState state, Dictionary<long, CelestialObject> satellites, Dictionary<long, Structure> structures)
			{
				if ( state.Structures == null )
					return;

				var query =
					(
						from s in state.Structures
						let structure = structures[s.Id]
						select new
						{
							s.Id,
							s.OrbitsId,
							s.LocalCoordinates,
							Instance = structure,
						}
					);

				foreach ( var structure in query )
				{
					CelestialObject parent;
					if (!satellites.TryGetValue(structure.OrbitsId, out parent)) 
						continue;

					parent.Structures.Add(structure.Instance);
					structure.Instance.Position = new Position(parent, structure.LocalCoordinates);

					var orbitDistance = structure.LocalCoordinates.Magnitude;
					CheckOrbitDistance(structure.Instance, parent, orbitDistance);
				}
			}

			private static void AssignSatellites(SolarSystemState state, Dictionary<long, CelestialObject> satellites)
			{
				if ( state.Satellites == null )
					return;

				var query =
					(
						from s in state.Satellites
						let satellite = satellites[s.Id]
						select new
						{
							s.Id,
							s.OrbitsId,
							s.LocalCoordinates,
							Instance = satellite,
						}
					);

				foreach (var satellite in query)
				{
					CelestialObject parent;
					if (!satellites.TryGetValue(satellite.OrbitsId, out parent)) 
						continue;

					parent.Satellites.Add(satellite.Instance);
					satellite.Instance.Position = new Position(parent, satellite.LocalCoordinates);

					var orbitDistance = satellite.LocalCoordinates.Magnitude - satellite.Instance.Size;
					CheckOrbitDistance(satellite.Instance, parent, orbitDistance);
				}
			}

			private static void CheckOrbitDistance(OrbitingObject @object, CelestialObject parent, double orbitDistance)
			{
				if (orbitDistance <= parent.Size)
				{
					throw new InvalidOperationException(
						string.Format("{0} cannot orbit {1} as the orbit is decaying", @object.Name, parent.Name));
				}
			}
		}
	}
}