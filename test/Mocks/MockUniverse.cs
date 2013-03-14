using System;
using System.Linq;
using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using Echo.State.Market;
using Echo.Statistics;
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

		public MockUniverse(IIdGenerator idGenerator = null)
		{
			_idGenerator = idGenerator ?? new IdGenerator();
			var universeId = Id();

			SpaceshipCommand = TestSkills.For(SkillCode.SpaceshipCommand);
			BluePrint = TestItems.BluePrint(ItemCode.MissileLauncher);
			Weapon = TestItems.Weapon(ItemCode.MissileLauncher);
			Item = TestItems.Item(ItemCode.Veldnium);

			John = new AgentState
			{
				ObjectId = Id(),
				Name = "John",
				Statistics = Enum.GetValues(typeof(AgentStatistic)).Cast<AgentStatistic>().Select(x => new AgentStatisticState { Statistic = x, CurrentValue = 50, Value = 50 }),
				Implants = new[] { AgentStatistic.Intelligence, AgentStatistic.Willpower, }.Select(x => new Implant { Stat = x, Rank = 3, Value = 15 }),
				Skills = new[] { new SkillLevel { SkillCode = SkillCode.SpaceshipCommand, Level = 5, } }
			};

			MSCorp = new CorporationState
			{
				ObjectId = Id(),
				Name = "MS",
				Employees = new[] { John }
			};

			Earth = new CelestialObjectState
			{
				ObjectId = Id(),
				CelestialObjectType = CelestialObjectType.Planet,
				Name = "Earth",
				Mass = 50d,
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
				TradingStation = new TradingStationState()
				{
					BuyOrders = new[] { new BuyOrderState { Auction = new AuctionState {} }, },
					SellOrders = new[] { new SellOrderState { Auction = new AuctionState { } }, }
				},
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
			};

			Universe = new UniverseState
			{
				ObjectId = universeId,
				StarClusters = new[] {StarCluster},
				Weapons = new[] { Weapon },
				Skills = TestSkills.Skills,
				Corporations = new[] { MSCorp },
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
							new State.SkillLevel { Level  = 1, SkillCode = SkillCode.SpaceshipCommand },
						}
					}
				}
			};

			BluePrint = new BluePrintInfo(ItemCode.MiningLaser)
			{
				BuildRequirements = new[] { new SkillLevel { SkillCode = SkillCode.SpaceshipCommand, Level = 5 }, },
				Materials = new[] { new ItemState { Code = ItemCode.Veldnium, Quantity = 10 }, },
				TargetQuantity = 1,
			};
		}

		private long Id()
		{
			return _idGenerator.NextId();
		}
	}
}