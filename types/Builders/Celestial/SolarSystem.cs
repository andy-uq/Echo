using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.State;
using Echo.Structures;
using Echo;

namespace Echo.Celestial
{
	partial class SolarSystem
	{
		public class Builder
		{
			public Builder()
			{
			}

			public SolarSystem Build(StarCluster starCluster, SolarSystemState state)
			{
				var solarSystem = new SolarSystem
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(starCluster, state.LocalCoordinates),
				};

				solarSystem.Satellites = state.Satellites
					.Select(x => CelestialObject.Builder.For(x).Build(solarSystem, x))
					.ToList();

				solarSystem.Structures = state.Structures
					.Select(x => Structure.Builder.For(x).Build(solarSystem, x))
					.ToList();

				var satellites = Enumerable.ToDictionary<CelestialObject, long>(solarSystem.Satellites, x => x.Id);
				var structures = Enumerable.ToDictionary<Structure, long>(solarSystem.Structures, x => x.Id);

				AssignSatellites(state, satellites);
				AssignStructures(state, satellites, structures);

				return solarSystem;
			}

			private void AssignStructures(SolarSystemState state, Dictionary<long, CelestialObject> satellites, Dictionary<long, Structure> structures)
			{
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
					if ( satellites.TryGetValue(structure.OrbitsId, out parent) )
					{
						parent.Structures.Add(structure.Instance);
						structure.Instance.Position = new Position(parent, structure.LocalCoordinates);

						var orbitDistance = structure.LocalCoordinates.Magnitude;
						CheckOrbitDistance(structure.Instance, parent, orbitDistance);
					}
				}
			}

			private static void AssignSatellites(SolarSystemState state, Dictionary<long, CelestialObject> satellites)
			{
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
					if (satellites.TryGetValue(satellite.OrbitsId, out parent))
					{
						parent.Satellites.Add(satellite.Instance);
						satellite.Instance.Position = new Position(parent, satellite.LocalCoordinates);

						var orbitDistance = satellite.LocalCoordinates.Magnitude - satellite.Instance.Size;
						CheckOrbitDistance(satellite.Instance, parent, orbitDistance);
					}
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