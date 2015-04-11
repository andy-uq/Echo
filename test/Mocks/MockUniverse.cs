using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.Market;
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
		public SkillInfo SpaceshipCommand { get; set; }
		public ShipState Ship { get; set; }
		public AgentState John { get; set; }
		public CorporationState MSCorp { get; set; }
		public CorporationState AppleCorp { get; set; }
		public StructureState Manufactory { get; set; }
		public StructureState TradingStation { get; set; }
		public CelestialObjectState Moon { get; set; }
		public CelestialObjectState Earth { get; set; }
		public CelestialObjectState AsteroidBelt { get; set; }
		public SolarSystemState SolarSystem { get; set; }
		public StarClusterState StarCluster { get; set; }
		public UniverseState Universe { get; set; }

		public ItemInfo Item { get; set; }
		public WeaponInfo Weapon { get; set; }
		public BluePrintInfo BluePrint { get; set; }
		public BluePrintInfo ShipBluePrint { get; set; }

		public AuctionState BuyOrder { get; set; }
		public AuctionState SellOrder { get; set; }

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
				Statistics = Enum.GetValues(typeof(AgentStatistic)).Cast<AgentStatistic>().Select(x => new AgentStatisticState { Statistic = x, CurrentValue = 50, Value = 50 }),
				Implants = new[] { AgentStatistic.Intelligence, AgentStatistic.Willpower, }.Select(x => new Implant { Stat = x, Rank = 3, Value = 15 }),
				Skills = new[] { new SkillLevel(SkillCode.SpaceshipCommand, level: 5) }
			};

			MSCorp = new CorporationState
			{
				ObjectId = Id(),
				Name = "MS",
				Employees = new[] { John }
			};

			AppleCorp = new CorporationState
			{
				ObjectId = Id(),
				Name = "Apple",
			};

			BuyOrder = new AuctionState
			{
				ObjectId = Id(),
				PricePerUnit = 5,
				Trader = John.ToObjectReference(),
				Item = new ItemState { Code = ItemCode.Veldnium, Quantity = 50 },
				Owner = MSCorp.ToObjectReference(),
			};

			SellOrder = new AuctionState
			{
				ObjectId = Id(),
				PricePerUnit = 10,
				Trader = John.ToObjectReference(),
				Item = new ItemState { Code = ItemCode.Veldnium, Quantity = 100 },
				Owner = MSCorp.ToObjectReference(),
			};

			Earth = new CelestialObjectState
			{
				ObjectId = Id(),
				CelestialObjectType = CelestialObjectType.Planet,
				Name = "Earth",
				LocalCoordinates = new Vector(Units.FromAU(1), 0),
				Mass = Units.SolarMass*1E-6,
				Size = 5d,
			};
			AsteroidBelt = new CelestialObjectState
			{
				ObjectId = Id(),
				CelestialObjectType = CelestialObjectType.AsteriodBelt,
				Name = "Asteroid Belt",
				Orbits = Earth.ToObjectReference(),
				LocalCoordinates = new Vector(-5.5, 0, 0),
				AsteroidBelt = new AsteroidBeltState
				{
					Richness = 500000,
					AmountRemaining = 250000,
				},
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
			Manufactory = new StructureState()
			{
				ObjectId = Id(),
				Name = "MFC",
				Orbits = Moon.ToObjectReference(),
				LocalCoordinates = new Vector(0.5001, 0, 0),
				Owner = MSCorp.ToObjectReference(),
				Manufactory = new ManufactoryState() { Efficiency = 0.5d },
			};
			TradingStation = new StructureState()
			{
				ObjectId = Id(),
				Name = "TS",
				Orbits = Moon.ToObjectReference(),
				LocalCoordinates = new Vector(-0.5001, 0, 0),
				Owner = MSCorp.ToObjectReference(),
				HangerItems = new[] { new HangarItemState { Owner = MSCorp.ToObjectReference(), Items = new[] { ItemCode.MissileLauncher.ToItemState(quantity:10) }  }, },
				BuyOrders = new[] { BuyOrder },
				SellOrders = new[] { SellOrder },
				TradingStation = new TradingStationState()
			};

			Ship = new ShipState()
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
							Code = Weapon.Code,
						}
					},
				},
				Pilot = John,
				Statistics = Enum.GetValues(typeof(ShipStatistic)).Cast<ShipStatistic>().Select(x => new ShipStatisticState { Statistic = x, CurrentValue = 50, Value = 50 }),
			};
			SolarSystem = new SolarSystemState
			{
				ObjectId = Id(),
				Name = "Sol",
				Satellites = new[] { Earth, Moon, AsteroidBelt, },
				Structures = new[] { Manufactory, TradingStation },
				Ships = new[] { Ship }
			};
			StarCluster = new StarClusterState
			{
				ObjectId = Id(),
				Name = "Revvon",
				SolarSystems = new[] { SolarSystem },
				MarketPlace = new MarketPlaceState
				{
					AuctionLength = 10,
					SettlementDelay = 5,
					Settlements = new[]
					{
						new SettlementState
						{
							ObjectId = Id(),
							Item = new ItemState { Code = ItemCode.Veldnium, Quantity = 50 }, 
							Owner = MSCorp.ToObjectReference(), 
							Location = TradingStation.ToObjectReference(),
							TimeToSettlement = 100, 
							SpendByOwner = new Dictionary<ObjectReference, long> { { AppleCorp.ToObjectReference(), 1000 } },
						}
					},
					Auctions = new[] { BuyOrder.ToObjectReference(), SellOrder.ToObjectReference() },
				}
			};

			Universe = new UniverseState
			{
				ObjectId = universeId,
				StarClusters = new[] {StarCluster},
				Weapons = new[] { Weapon },
				Skills = TestSkills.Skills,
				Corporations = new[] { MSCorp, AppleCorp },
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
							new State.SkillLevel(SkillCode.SpaceshipCommand, level:1),
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