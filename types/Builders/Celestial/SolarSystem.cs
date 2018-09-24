using System;
using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.JumpGates;
using Echo.Ships;
using Echo.State;
using Echo.Structures;

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
					Ships = solarSystem.Ships.Save()
				};
			}

			public static ObjectBuilder<SolarSystem> Build(StarCluster starCluster, SolarSystemState state)
			{
				var solarSystem = new SolarSystem
				{
					Id = state.ObjectId,
					Name = state.Name,
					Position = new Position(starCluster, state.LocalCoordinates)
				};

				var builder = new ObjectBuilder<SolarSystem>(solarSystem)
					.Resolve((target, resolver) => BuildOrbits(state, target));

				builder
					.Dependents(state.Ships)
					.Build(Ship.Builder.Build)
					.Resolve((resolver, target, dependent) => target.Ships.Add(dependent));
				
				builder
					.Dependents(state.JumpGates)
					.Build(JumpGate.Builder.Build)
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
				if (resolver == null)
					throw new ArgumentNullException(nameof(resolver));

				var query =
					(
						from s in state.Structures
						let structure = resolver.GetById<Structure>(s.ObjectId)
						select new
						{
							s.ObjectId,
							s.Orbits,
							s.LocalCoordinates,
							Instance = structure
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
							Instance = satellite
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
				if (resolver.TryGet(orbits, out CelestialObject parent))
				{
					return parent;
				}

				throw new InvalidOperationException($"{satellite.AsObjectReference()} is not orbiting anything");
			}

			private static void CheckOrbitDistance(OrbitingObject @object, CelestialObject parent, double orbitDistance)
			{
				if (orbitDistance > parent.Size)
				{
					return;
				}
				
				throw new InvalidOperationException(
					$"{@object.Name} cannot orbit {parent.Name} as the orbit is decaying");
			}
		}
	}
}