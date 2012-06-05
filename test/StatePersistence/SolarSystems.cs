using System;
using System.Linq;
using Echo.Celestial;
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
			var solarSystem = Echo.Celestial.SolarSystem.Builder.Build(null, SolarSystem).Materialise();
			var state = Echo.Celestial.SolarSystem.Builder.Save(solarSystem);

			Console.WriteLine(state.SerializeAndFormat());
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(SolarSystem);
			var state = Database.UseOnceTo().GetById<SolarSystemState>(1L);
			Assert.That(state, Is.Not.Null);

			var solarSystem = Echo.Celestial.SolarSystem.Builder.Build(null, state).Materialise();
			
			var earth = solarSystem.Satellites.OfType<Planet>().Single(x => x.Id == Earth.Id);
			Assert.That(earth.Sun, Is.EqualTo(solarSystem));
			
			var asteroidBelt = solarSystem.Satellites.OfType<AsteroidBelt>().Single(x => x.Id == AsteroidBelt.Id);
			Assert.That(asteroidBelt.Position.Location, Is.EqualTo(earth));

			var moon = solarSystem.Satellites.OfType<Moon>().Single(x => x.Id == Moon.Id);
			Assert.That(moon.Planet, Is.EqualTo(earth));

			var manufactory = solarSystem.Structures.Single(x => x.Id == Manufactory.Id);
			Assert.That(manufactory.Position.Location, Is.EqualTo(moon));
		}
	}
}