using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using Echo.State.Market;
using Echo.Statistics;
using test.common;
using SkillLevel = Echo.State.SkillLevel;

namespace Echo.Tests.Mocks
{
	public class MockUniverse
	{
		private readonly IIdGenerator _idGenerator;
		public SkillInfo SpaceshipCommand { get; }
		public ShipState Ship { get; }
		public AgentState John { get; }
		public CorporationState MSCorp { get; }
		public CorporationState AppleCorp { get; }
		public StructureState Manufactory { get; }
		public StructureState TradingStation { get; }
		public CelestialObjectState Moon { get; }
		public CelestialObjectState Earth { get; }
		public CelestialObjectState AsteroidBelt { get; }
		public SolarSystemState SolarSystem { get; }
		public StarClusterState StarCluster { get; }
		public UniverseState Universe { get; }

		public ItemInfo Item { get; }
		public WeaponInfo Weapon { get; }
		public BluePrintInfo BluePrint { get; }
		public BluePrintInfo ShipBluePrint { get; }

		public AuctionState BuyOrder { get; }
		public AuctionState SellOrder { get; }

		public MockUniverse(IIdGenerator idGenerator = null)
		{
			_idGenerator = idGenerator ?? new IdGenerator();
			var universeId = Id();

			SpaceshipCommand = TestSkills.For(SkillCode.SpaceshipCommand);
			BluePrint = TestItems.BluePrint(ItemCode.MissileLauncher);
			ShipBluePrint = TestItems.BluePrint(ItemCode.LightFrigate);
			Weapon = TestItems.Weapon(ItemCode.MissileLauncher);
			Item = TestItems.Item(ItemCode.Veldnium);

			John = new AgentState
			{
				ObjectId = Id(),
				Name = "John",
				Statistics = Enum.GetValues(typeof(AgentStatistic)).Cast<AgentStatistic>().Select(x => new AgentStatisticState {Statistic = x, CurrentValue = 50, Value = 50}),
				Implants = new[] {AgentStatistic.Intelligence, AgentStatistic.Willpower}.Select(x => new State.Implant { Stat = x, Rank = 3, Value = 15}),
				Skills = new[] {new SkillLevel(SkillCode.SpaceshipCommand, level: 5)}
			};

			MSCorp = new CorporationState
			{
				ObjectId = Id(),
				Name = "MS",
				Employees = new[] {John}
			};

			AppleCorp = new CorporationState
			{
				ObjectId = Id(),
				Name = "Apple"
			};

			BuyOrder = new AuctionState
			{
				ObjectId = Id(),
				PricePerUnit = 5,
				Trader = John.ToObjectReference(),
				Item = new ItemState {Code = ItemCode.Veldnium, Quantity = 50},
				Owner = MSCorp.ToObjectReference()
			};

			SellOrder = new AuctionState
			{
				ObjectId = Id(),
				PricePerUnit = 10,
				Trader = John.ToObjectReference(),
				Item = new ItemState {Code = ItemCode.Veldnium, Quantity = 100},
				Owner = MSCorp.ToObjectReference()
			};

			Earth = new CelestialObjectState
			{
				ObjectId = Id(),
				CelestialObjectType = CelestialObjectType.Planet,
				Name = "Earth",
				LocalCoordinates = new Vector(Units.FromAU(1), 0),
				Mass = Units.SolarMass * 1E-6,
				Size = 5d
			};
			AsteroidBelt = new CelestialObjectState
			{
				ObjectId = Id(),
				CelestialObjectType = CelestialObjectType.AsteroidBelt,
				Name = "Asteroid Belt",
				Orbits = Earth.ToObjectReference(),
				LocalCoordinates = new Vector(-5.5, 0, 0),
				AsteroidBelt = new AsteroidBeltState
				{
					Richness = 500000,
					AmountRemaining = 250000
				}
			};
			Moon = new CelestialObjectState
			{
				ObjectId = Id(),
				CelestialObjectType = CelestialObjectType.Moon,
				Name = "Moon",
				Orbits = Earth.ToObjectReference(),
				Mass = 0.5d,
				Size = 0.5d,
				LocalCoordinates = new Vector(7.5, 0, 0)
			};
			Manufactory = new StructureState
			{
				ObjectId = Id(),
				Name = "MFC",
				Orbits = Moon.ToObjectReference(),
				LocalCoordinates = new Vector(0.5001, 0, 0),
				Owner = MSCorp.ToObjectReference(),
				Manufactory = new ManufactoryState {Efficiency = 0.5d}
			};
			TradingStation = new StructureState
			{
				ObjectId = Id(),
				Name = "TS",
				Orbits = Moon.ToObjectReference(),
				LocalCoordinates = new Vector(-0.5001, 0, 0),
				Owner = MSCorp.ToObjectReference(),
				HangerItems = new[]
				{
					new HangarItemState
					{
						Owner = MSCorp.ToObjectReference(),
						Items = new[] {ItemCode.MissileLauncher.ToItemState(quantity: 10)}
					}
				},
				BuyOrders = new[] {BuyOrder},
				SellOrders = new[] {SellOrder},
				TradingStation = new TradingStationState()
			};

			Ship = new ShipState
			{
				ObjectId = Id(),
				Name = "Ship",
				LocalCoordinates = new Vector(8.5, 0, 0),
				Code = ItemCode.LightFrigate,
				HardPoints = new[]
				{
					new HardPointState
					{
						Position = HardPointPosition.Front,
						Orientation = HardPoint.CalculateOrientation(HardPointPosition.Front),
						Speed = 0.5d,
						Weapon = new WeaponState
						{
							ObjectId = Id(),
							Name = "Blaster",
							Code = Weapon.Code
						}
					}
				},
				Pilot = John,
				Statistics = Enum.GetValues(typeof(ShipStatistic)).Cast<ShipStatistic>().Select(x =>
					new ShipStatisticState {Statistic = x, CurrentValue = 50, Value = 50})
			};
			SolarSystem = new SolarSystemState
			{
				ObjectId = Id(),
				Name = "Sol",
				Satellites = new[] {Earth, Moon, AsteroidBelt},
				Structures = new[] {Manufactory, TradingStation},
				Ships = new[] {Ship}
			};
			StarCluster = new StarClusterState
			{
				ObjectId = Id(),
				Name = "Revvon",
				SolarSystems = new[] {SolarSystem},
				MarketPlace = new MarketPlaceState
				{
					AuctionLength = 10,
					SettlementDelay = 5,
					Settlements = new[]
					{
						new SettlementState
						{
							ObjectId = Id(),
							Item = new ItemState {Code = ItemCode.Veldnium, Quantity = 50},
							Owner = MSCorp.ToObjectReference(),
							Location = TradingStation.ToObjectReference(),
							TimeToSettlement = 100,
							SpendByOwner = new Dictionary<ObjectReference, long> {{AppleCorp.ToObjectReference(), 1000}}
						}
					},
					Auctions = new[] {BuyOrder.ToObjectReference(), SellOrder.ToObjectReference()}
				}
			};

			Universe = new UniverseState
			{
				ObjectId = universeId,
				StarClusters = new[] {StarCluster},
				Weapons = new[] {Weapon},
				Skills = TestSkills.Skills,
				Corporations = new[] {MSCorp, AppleCorp},
				Items = TestItems.Items,
				BluePrints = TestItems.BluePrints,
				Ships = new[]
				{
					new ShipInfo
					{
						Code = ItemCode.LightFrigate,
						ShipClass = ShipClass.LightFrigate,
						PilotRequirements = new[]
						{
							new SkillLevel(SkillCode.SpaceshipCommand, level: 1)
						}
					}
				}
			};
		}

		private ulong Id()
		{
			return _idGenerator.NextId();
		}
	}
}