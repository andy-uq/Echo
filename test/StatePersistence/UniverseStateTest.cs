using System;
using System.Linq;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using NUnit.Framework;
using Echo;

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
			var json = Database.Serializer.Serialize(universe);
			Console.WriteLine(json);
		}

		[Test]
		public void Save()
		{
			var universe = Echo.Universe.Builder.Build(GetUniverse()).Materialise();
			var state = universe.Save();

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}
		
		[Test]
		public void Persist()
		{
			SaveToDb();

			DumpObjects("StarCluster");
			DumpObjects("Corporation");
			DumpObjects("Item", isInfo:true);
			DumpObjects("Skill", isInfo: true);
		}

		private void SaveToDb()
		{
			var universe = GetUniverse();

			using (var session = Database.BeginSession())
			{
				session.InsertMany(universe.StarClusters);
				session.InsertMany(universe.Corporations);
				session.InsertMany(universe.Items);
				session.InsertMany(universe.Skills);
			}
		}

		[Test]
		public void LoadFromDb()
		{
			SaveToDb();

			using ( var session = Database.BeginSession() )
			{
				var universe = new UniverseState
				{
					StarClusters = session.Query<StarClusterState>().ToArray(),
					Corporations = session.Query<CorporationState>().ToArray(),
					Items = session.Query<ItemInfo>().ToArray(),
					Skills = session.Query<SkillInfo>().ToArray()
				};

				Check(universe.StarClusters.First());
			}
		}

		[Test]
		public void Deserialise()
		{
			var universe = GetUniverse();
			Database.UseOnceTo().InsertMany(universe.StarClusters);

			var state = Database.UseOnceTo().Query<StarClusterState>().First();
			Check(state);
		}

		private static void Check(StarClusterState state)
		{
			SolarSystemState solState = state.SolarSystems.First();
			Assert.That(solState, Is.Not.Null);

			var earthState = solState.Satellites.FirstOrDefault();
			Assert.That(earthState, Is.Not.Null);
			Assert.That(earthState, Is.InstanceOf<CelestialObjectState>());

			var starCluster = StarCluster.Builder.Build(null, state).Materialise();

			Assert.That(starCluster, Is.Not.Null);
			Assert.That(starCluster.SolarSystems, Is.Not.Empty);

			SolarSystem sol = starCluster.SolarSystems.First();

			var earth = sol.Satellites.Single(x => x.Name == "Earth");
			Assert.That(earth, Is.InstanceOf<Planet>());

			var moon = earth.Satellites.Single(x => x.Name == "Moon");
			Assert.That(moon, Is.InstanceOf<Moon>());
		}
	}
}