using System;
using System.Linq;
using Echo.Builder;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Items;
using Echo.State;
using Echo.Tests;
using Echo.Tests.Mocks;
using Echo.Tests.StatePersistence;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
{
	[TestFixture]
	public class SolarSystemStateTest : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
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
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(SolarSystem));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var builder = Echo.Celestial.SolarSystem.Builder.Build(null, SolarSystem);
			builder.Dependent(new ShipInfo { Code = ItemCode.LightFrigate }).Build(x => new ObjectBuilder<ShipInfo>(x));
			builder.Dependent(Universe.Weapon).Build(x => new ObjectBuilder<WeaponInfo>(x));
			builder.Dependent(Universe.MSCorp).Build(Corporation.Builder.Build);

			var solarSystem = builder.Materialise();
			Check(solarSystem);

			var state = Echo.Celestial.SolarSystem.Builder.Save(solarSystem);

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);

			Check(state);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(SolarSystem);
			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();
			}

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);

				var asteroidBeltState = state.Satellites.Single(x => x.ObjectId == Universe.AsteroidBelt.ObjectId);
				Assert.That(asteroidBeltState.Orbits, Is.Not.Null);

				var moonState = state.Satellites.Single(x => x.ObjectId == Universe.Moon.ObjectId);
				Assert.That(moonState.Orbits, Is.Not.Null);

				Check(state);
			}
		}

		private void Check(SolarSystemState state)
		{
			var builder = Echo.Celestial.SolarSystem.Builder.Build(null, state);
			builder.Dependent(new ShipInfo { Code = ItemCode.LightFrigate }).Build(x => new ObjectBuilder<ShipInfo>(x));
			builder.Dependent(Universe.Weapon).Build(x => new ObjectBuilder<WeaponInfo>(x));
			builder.Dependent(Universe.MSCorp).Build(Corporation.Builder.Build);

			var solarSystem = builder.Materialise();
			Check(solarSystem);
		}

		private void Check(SolarSystem solarSystem)
		{
			Assert.That(solarSystem.Satellites, Is.Not.Empty);

			Assert.That(solarSystem.Satellites.OfType<Planet>(), Is.Not.Empty);
			var earth = solarSystem.Satellites.OfType<Planet>().Single(x => x.Id == Earth.ObjectId);
			Assert.That(earth.Sun, Is.EqualTo(solarSystem));

			Assert.That(solarSystem.Satellites.OfType<Moon>(), Is.Not.Empty);
			var moon = solarSystem.Satellites.OfType<Moon>().Single(x => x.Id == Moon.ObjectId);
			Assert.That(moon.Planet, Is.EqualTo(earth));

			var manufactory = solarSystem.Structures.Single(x => x.Id == Manufactory.ObjectId);
			Assert.That(manufactory.Position.Location, Is.EqualTo(moon));

			var asteroidBelt = solarSystem.Satellites.Single(x => x.Id == AsteroidBelt.ObjectId);
			Assert.That(asteroidBelt.Position.Location, Is.EqualTo(earth));
		}
	}
}