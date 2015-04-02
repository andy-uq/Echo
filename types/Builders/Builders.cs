using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.Ships;
using Echo.State;
using Echo.State.Market;
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
			public static CelestialObjectState Save(this CelestialObject celestialObject)
			{
				return CelestialObject.Builder.For(celestialObject).Save(celestialObject);
			}

			public static StructureState Save(this Structure structure)
			{
				return Structure.Builder.For(structure).Save(structure);
			}
	
			public static AgentState Save(this Agent agent)
			{
				if ( agent == null )
					return null;

				return Agent.Builder.Save(agent);
			}
	
			public static CorporationState Save(this Corporation corporation)
			{
				if ( corporation == null )
					return null;

				return Corporation.Builder.Save(corporation);
			}

			public static WeaponState Save(this Weapon weapon)
			{
				if ( weapon == null )
					return null;

				return Weapon.Builder.Save(weapon);
			}

			public static ItemState Save(this Item item)
			{
				return Item.Builder.Save(item);
			}

			public static MarketPlaceState Save(this MarketPlace marketPlace)
			{
				return MarketPlace.Builder.Save(marketPlace);
			}

			public static AuctionState Save(this Auction auction)
			{
				return Auction.Builder.Save(auction);
			}

			public static IEnumerable<SettlementState> Save(this IEnumerable<Settlement> settlements)
			{
				return settlements.Select(Settlement.Builder.Save);
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

			public static IEnumerable<CorporationState> Save(this IEnumerable<Corporation> corporations)
			{
				return corporations.Select(Corporation.Builder.Save);
			}

			public static IEnumerable<HardPointState> Save(this IEnumerable<HardPoint> hardPoints)
			{
				return hardPoints.Select(HardPoint.Builder.Save);
			}
		}
	}
}