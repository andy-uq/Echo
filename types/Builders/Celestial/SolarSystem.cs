using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.JumpGates;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using Echo;
using EnsureThat;

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
					ObjectId = solarSystem.Id,
					Name = solarSystem.Name,
					LocalCoordinates = solarSystem.Position.LocalCoordinates,
					Satellites = solarSystem.Satellites.Save(),
					Structures = solarSystem.Structures.Save(),
					Ships = solarSystem.Ships.Save(),
				};
			}

			public static ObjectBuilder<SolarSystem> Build(StarCluster starCluster, SolarSystemState state)
			{
				var solarSystem = new SolarSystem
				{
					Id = state.ObjectId,
					Name = state.Name,
					Position = new Position(starCluster, state.LocalCoordinates),
				};

				var builder = new ObjectBuilder<SolarSystem>(solarSystem)
					.Resolve((target, resolver) => BuildOrbits(state, target));

				builder
					.Dependents(state.Ships)
					.Build(Echo.Ships.Ship.Builder.Build)
					.Resolve((resolver, target, dependent) => target.Ships.Add(dependent));
				
				builder
					.Dependents(state.JumpGates)
					.Build(Echo.JumpGates.JumpGate.Builder.Build)
					.Resolve((resolver, target, dependent) => target.JumpGates.Add(dependent));

				builder
					.Dependents(state.Satellites)
					.Build((target, dependent) => CelestialObject.Builder.For(dependent).Build(solarSystem))
					.Resolve((resolver, target, dependent) => target.Satellites.Add(dependent));

				builder
					.Dependents(state.Structures)
					.Build((target, dependent) => Structure.Builder.For(dependent).Build(solarSystem))
					.Resolve((resolver, target, dependent) => target.Structures.Add(dependent));

				return builder;
			}
			
			private static void BuildOrbits(SolarSystemState arg1, IIdResolver arg2)
			{
				BuildSatelliteOrbits(arg1, arg2);
				BuildStructureOrbits(arg1, arg2);
			}

			private static void BuildStructureOrbits(SolarSystemState state, IIdResolver resolver)
			{
				Ensure.That(resolver).IsNotNull();

				if ( state.Structures == null )
					return;

				var query =
					(
						from s in state.Structures
						let structure = resolver.GetById<Structure>(s.ObjectId)
						select new
						{
							s.ObjectId,
							s.Orbits,
							s.LocalCoordinates,
							Instance = structure,
						}
					);

				foreach ( var structure in query )
				{
					var parent = GetParent(resolver, structure.Instance, structure.Orbits);

					parent.Structures.Add(structure.Instance);
					structure.Instance.Position = new Position(parent, structure.LocalCoordinates);

					var orbitDistance = structure.LocalCoordinates.Magnitude;
					CheckOrbitDistance(structure.Instance, parent, orbitDistance);
				}
			}

			private static void BuildSatelliteOrbits(SolarSystemState state, IIdResolver resolver)
			{
				Ensure.That(resolver).IsNotNull();

				if ( state.Satellites == null )
					return;

				var query =
					(
						from s in state.Satellites
						let satellite = resolver.GetById<CelestialObject>(s.ObjectId)
						where s.Orbits.HasValue
						select new
						{
							s.ObjectId,
							s.Orbits,
							s.LocalCoordinates,
							Instance = satellite,
						}
					);

				foreach (var satellite in query)
				{
					var parent = GetParent(resolver, satellite.Instance, satellite.Orbits);

					parent.Satellites.Add(satellite.Instance);
					satellite.Instance.Position = new Position(parent, satellite.LocalCoordinates);

					var orbitDistance = satellite.LocalCoordinates.Magnitude - satellite.Instance.Size;
					CheckOrbitDistance(satellite.Instance, parent, orbitDistance);
				}
			}

			private static CelestialObject GetParent(IIdResolver resolver, IObject satellite, ObjectReference? orbits)
			{
				CelestialObject parent;
				if (resolver.TryGet(orbits, out parent))
				{
					return parent;
				}

				throw new InvalidOperationException(string.Format("{0} is not orbiting anything", satellite.AsObjectReference()));
			}

			private static void CheckOrbitDistance(OrbitingObject @object, CelestialObject parent, double orbitDistance)
			{
				if (orbitDistance > parent.Size)
				{
					return;
				}
				
				throw new InvalidOperationException(
					string.Format("{0} cannot orbit {1} as the orbit is decaying", @object.Name, parent.Name));
			}
		}
	}
}