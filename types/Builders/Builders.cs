using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Ships;
using Echo.State;
using Echo.Structures;

namespace Echo
{
	namespace Builders
	{
		public static class BuilderExtensions
		{
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