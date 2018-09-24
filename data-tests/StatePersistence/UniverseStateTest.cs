using System;
using System.Linq;
using Echo.Builders;
using Echo.Celestial;
using Echo.Data;
using Echo.State;
using Echo.Statistics;
using Echo.Structures;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class UniverseStateTest : StateTest
	{
		private UniverseState GetUniverse()
		{
			return Universe.Universe;
		}

		[Test]
		public void MaterialiseUniverse()
		{
			var state = GetUniverse();
			var u = Echo.Universe.Builder.Build(state);
		}

		[Test]
		public void Serialise()
		{
			var universe = GetUniverse();

			var jsonSerializer = Database.Conventions.CreateSerializer();

			var statistic = new AgentStatisticState { Statistic = AgentStatistic.Intelligence, Value = 100, CurrentValue = 10 };
			var agent = new AgentState { Statistics = new[] { statistic }.Where(x => x.CurrentValue > 0) };
			jsonSerializer.Serialize(agent.Statistics);

			var json = jsonSerializer.Serialize(universe);
			Console.WriteLine(json);
		}

		[Test]
		public void Save()
		{
			var universe = Echo.Universe.Builder.Build(GetUniverse()).Materialise();
			var state = universe.Save();

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}
		
		[Test]
		public void Persist()
		{
			SaveToDb();

			DumpObjects("StarCluster");
			DumpObjects("Corporation");
			DumpObjects("Item", isInfo:true);
			DumpObjects("Weapon", isInfo:true);
			DumpObjects("Skill", isInfo: true);
		}

		private void SaveToDb()
		{
			var universe = GetUniverse();

			Assert.That(universe.StarClusters, Is.Not.Empty);
			Assert.That(universe.Corporations, Is.Not.Empty);
			Assert.That(universe.Items, Is.Not.Empty);
			Assert.That(universe.Weapons, Is.Not.Empty);
			Assert.That(universe.Skills, Is.Not.Empty);
			Assert.That(universe.Ships, Is.Not.Empty);


			using (var session = Database.OpenSession())
			{
				session.StoreMany(universe.StarClusters);
				session.StoreMany(universe.Corporations);
				session.StoreMany(universe.Items, "Item", x => x.Code);
				session.StoreMany(universe.Weapons, "Weapon", x => x.Code);
				session.StoreMany(universe.Skills, "Skill", x => x.Code);
				session.StoreMany(universe.Ships, "Ship", x => x.Code);
				session.StoreMany(universe.BluePrints, "BluePrints", x => x.Code);

				session.SaveChanges();
			}
		}

		[Test]
		public void LoadFromDb()
		{
			SaveToDb();

			using ( var session = Database.OpenSession() )
			{
				var universe = new UniverseState
				{
					StarClusters = session.Query<StarClusterState>().ToArray(),
					Corporations = session.Query<CorporationState>().ToArray(),
					Items = session.Query<ItemInfo>().ToArray(),
					Weapons = session.Query<WeaponInfo>().ToArray(),
					Skills = session.Query<SkillInfo>().ToArray(),
					Ships = session.Query<ShipInfo>().ToArray(),
					BluePrints = session.Query<BluePrintInfo>().ToArray()
				};

				Assert.That(universe.StarClusters, Is.Not.Empty);
				Assert.That(universe.StarClusters.First().MarketPlace, Is.Not.Null);
				Assert.That(universe.Corporations, Is.Not.Empty);
				Assert.That(universe.Items, Is.Not.Empty);
				Assert.That(universe.Weapons, Is.Not.Empty);
				Assert.That(universe.Skills, Is.Not.Empty);
				Assert.That(universe.Ships, Is.Not.Empty);
				Assert.That(universe.BluePrints, Is.Not.Empty);

				Check(universe);
			}
		}

		[Test]
		public void Deserialise()
		{
			var universe = GetUniverse();
			Assert.That(universe.StarClusters, Is.Not.Empty);
			
			using ( var session = Database.OpenSession() )
			{
				session.StoreMany(universe.StarClusters);
				session.SaveChanges();
			}

			using ( var session = Database.OpenSession() )
			{
				universe.StarClusters = session.Query<StarClusterState>().ToList();
				Assert.That(universe.StarClusters, Is.Not.Empty);
				
				Check(universe);
			}
		}

		private static void Check(UniverseState state)
		{
			Assert.That(state.StarClusters, Is.Not.Empty);

			var starClusterState = state.StarClusters.First();
			var solState = starClusterState.SolarSystems.First();
			Assert.That(solState, Is.Not.Null);

			var earthState = solState.Satellites.FirstOrDefault();
			Assert.That(earthState, Is.Not.Null);
			Assert.That(earthState, Is.InstanceOf<CelestialObjectState>());

			var builder = Echo.Universe.Builder.Build(state);

			var universe = builder.Materialise();
			Assert.That(universe.StarClusters, Is.Not.Empty);

			var starCluster = universe.StarClusters.First();
			Assert.That(starCluster.SolarSystems, Is.Not.Empty);
			Assert.That(starCluster.MarketPlace, Is.Not.Null);
			
			var sol = starCluster.SolarSystems.First();

			var earth = sol.Satellites.Single(x => x.Name == "Earth");
			Assert.That(earth, Is.InstanceOf<Planet>());

			var moon = earth.Satellites.Single(x => x.Name == "Moon");
			Assert.That(moon, Is.InstanceOf<Moon>());

			var buyOrder = starCluster.MarketPlace.BuyOrders.First();
			var tradingStation = sol.Structures.OfType<TradingStation>().Single();

			Assert.That(tradingStation.BuyOrders.First(), Is.SameAs(buyOrder));
		}
	}
}