using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using Echo.Structures;

namespace Echo
{
	namespace Builders
	{
		public static class BuilderExtensions
		{
			public static UniverseState Save(this Universe universe)
			{
				return Universe.Builder.Save(universe);
			}

			public static CelestialObject Build(this CelestialObjectState state, ILocation location)
			{
				return CelestialObject.Builder.For(state).Build(location, state);
			}

			public static CelestialObjectState Save(this CelestialObject celestialObject)
			{
				return CelestialObject.Builder.For(celestialObject).Save(celestialObject);
			}

			public static Structure Build(this StructureState state, ILocation location)
			{
				return Structure.Builder.For(state).Build(location, state);
			}

			public static Item Build(this ItemState state, ILocation location, IIdResolver resolver)
			{
				return Item.Builder.Build(location, state, resolver);
			}

			public static IdResolutionContext<Item> Build(this ItemState state, ILocation location)
			{
				return Item.Builder.Build(location, state);
			}

			public static StructureState Save(this Structure structure)
			{
				return Structure.Builder.For(structure).Save(structure);
			}

			public static Weapon Build(this WeaponState state, ILocation location)
			{
				if ( state == null )
					return null;

				return Weapon.Builder.Build(location, state);
			}	
	
			public static WeaponState Save(this Weapon weapon)
			{
				if ( weapon == null )
					return null;

				return Weapon.Builder.Save(weapon);
			}
			
			public static Ship Build(this ShipState state, ILocation location)
			{
				return Ship.Builder.Build(location, state);
			}		
	
			public static IEnumerable<HardPoint> Build(this IEnumerable<HardPointState> states, Ship ship)
			{
				if (states == null)
					return Enumerable.Empty<HardPoint>();

				return states.Select(state => HardPoint.Builder.Build(ship, state));
			}

			public static IEnumerable<CelestialObjectState> Save(this IEnumerable<CelestialObject> celestialObjects)
			{
				return celestialObjects.Select(x => CelestialObject.Builder.For(x).Save(x));
			}

			public static IEnumerable<StructureState> Save(this IEnumerable<Structure> structures)
			{
				return structures.Select(x => Structure.Builder.For(x).Save(x));
			}

			public static IEnumerable<ShipState> Save(this IEnumerable<Ship> ships)
			{
				return ships.Select(Ship.Builder.Save);
			}

			public static SolarSystem Build(this SolarSystemState state, StarCluster starCluster)
			{
				return SolarSystem.Builder.Build(starCluster, state);
			}

			public static IEnumerable<SolarSystemState> Save(this IEnumerable<SolarSystem> solarSystems)
			{
				return solarSystems.Select(SolarSystem.Builder.Save);
			}

			public static IEnumerable<StarClusterState> Save(this IEnumerable<StarCluster> starClusters)
			{
				return starClusters.Select(StarCluster.Builder.Save);
			}

			public static IEnumerable<HardPointState> Save(this IEnumerable<HardPoint> hardPoints)
			{
				return hardPoints.Select(HardPoint.Builder.Save);
			}
		}
	}
}