using System;
using System.Linq;
using Echo.Builder;
using Echo.Celestial;
using Echo.Items;
using Echo.State;
using NUnit.Framework;
using Echo;
using SisoDb.Serialization;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class SolarSystems : StateTest
	{
		private SolarSystemState SolarSystem
		{
			get { return Universe.SolarSystem; }
		}

		private CelestialObjectState Earth
		{
			get { return Universe.Earth; }
		}

		private CelestialObjectState Moon
		{
			get { return Universe.Moon; }
		}

		private CelestialObjectState AsteroidBelt
		{
			get { return Universe.AsteroidBelt; }
		}

		private StructureState Manufactory
		{
			get { return Universe.Manufactory; }
		}


		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(SolarSystem);
			DumpObjects("SolarSystem");
		}

		[Test]
		public void Save()
		{
			var builder = Echo.Celestial.SolarSystem.Builder.Build(null, SolarSystem);
			builder.Dependent(new ShipInfo { Code = ItemCode.LightFrigate }).Build(x => new ObjectBuilder<ShipInfo>(x));

			var solarSystem = builder.Materialise();
			var state = Echo.Celestial.SolarSystem.Builder.Save(solarSystem);

			Console.WriteLine(state.SerializeAndFormat());
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(SolarSystem);
			var state = Database.UseOnceTo().GetById<SolarSystemState>(SolarSystem.Id);
			Assert.That(state, Is.Not.Null);

			var builder = Echo.Celestial.SolarSystem.Builder.Build(null, state);
			builder.Dependent(new ShipInfo {Code = ItemCode.LightFrigate}).Build(x => new ObjectBuilder<ShipInfo>(x));

			var solarSystem = builder.Materialise();

			var earth = solarSystem.Satellites.OfType<Planet>().Single(x => x.Id == Earth.ObjectId);
			Assert.That(earth.Sun, Is.EqualTo(solarSystem));

			var asteroidBelt = solarSystem.Satellites.OfType<AsteroidBelt>().Single(x => x.Id == AsteroidBelt.ObjectId);
			Assert.That(asteroidBelt.Position.Location, Is.EqualTo(earth));

			var moon = solarSystem.Satellites.OfType<Moon>().Single(x => x.Id == Moon.ObjectId);
			Assert.That(moon.Planet, Is.EqualTo(earth));

			var manufactory = solarSystem.Structures.Single(x => x.Id == Manufactory.ObjectId);
			Assert.That(manufactory.Position.Location, Is.EqualTo(moon));
		}
	}
}