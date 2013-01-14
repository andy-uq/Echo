using System;
using System.Linq;
using Echo.Builder;
using Echo.Celestial;
using Echo.Items;
using Echo.State;
using Echo.Tests;
using Echo.Tests.StatePersistence;
using NUnit.Framework;
using SisoDb.Serialization;

namespace Echo.Data.Tests.StatePersistence
{
	[TestFixture]
	public class SolarSystemStateTest : StateTest
	{
		public class WrappedObjectState
		{
			public Guid Id { get; set; }
			public SolarSystemState Value { get; set; }

			public WrappedObjectState(SolarSystemState value)
			{
				Value = value;
			}
		}
		
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
			Database.UseOnceTo().Insert(new WrappedObjectState(SolarSystem));
			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var builder = Echo.Celestial.SolarSystem.Builder.Build(null, SolarSystem);
			builder.Dependent(new ShipInfo { Code = ItemCode.LightFrigate }).Build(x => new ObjectBuilder<ShipInfo>(x));
			builder.Dependent(Universe.Weapon).Build(x => new ObjectBuilder<WeaponInfo>(x));

			var solarSystem = builder.Materialise();
			var state = Echo.Celestial.SolarSystem.Builder.Save(solarSystem);

			Console.WriteLine(state.SerializeAndFormat());
		}

		[Test]
		public void Deserialise()
		{
			var moonState = SolarSystem.Satellites.Single(x => x.ObjectId == Universe.Moon.ObjectId);
			Assert.That(moonState.Orbits, Is.Not.Null);

			var wrapped = new WrappedObjectState(SolarSystem);
			Database.UseOnceTo().Insert(wrapped);
			var state = Database.UseOnceTo().GetById<WrappedObjectState>(wrapped.Id).Value;
			Assert.That(state, Is.Not.Null);

			moonState = state.Satellites.Single(x => x.ObjectId == Universe.Moon.ObjectId);
			Assert.That(moonState.Orbits, Is.Not.Null);

			var builder = Echo.Celestial.SolarSystem.Builder.Build(null, state);
			builder.Dependent(new ShipInfo { Code = ItemCode.LightFrigate }).Build(x => new ObjectBuilder<ShipInfo>(x));
			builder.Dependent(Universe.Weapon).Build(x => new ObjectBuilder<WeaponInfo>(x));
			
			var solarSystem = builder.Materialise();

			var earth = solarSystem.Satellites.OfType<Planet>().Single(x => x.Id == Earth.ObjectId);
			Assert.That(earth.Sun, Is.EqualTo(solarSystem));

			var moon = solarSystem.Satellites.OfType<Moon>().Single(x => x.Id == Moon.ObjectId);
			Assert.That(moon.Planet, Is.EqualTo(earth));

			var manufactory = solarSystem.Structures.Single(x => x.Id == Manufactory.ObjectId);
			Assert.That(manufactory.Position.Location, Is.EqualTo(moon));

			var asteroidBelt = solarSystem.Satellites.Single(x => x.Id == AsteroidBelt.ObjectId);
			Assert.That(asteroidBelt.Position.Location, Is.EqualTo(earth));
		}
	}
}