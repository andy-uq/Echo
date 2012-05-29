using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class AsteroidBelts : StateTest
	{
		private CelestialObjectState AsteroidBelt
		{
			get { return Universe.AsteroidBelt; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(AsteroidBelt);
			DumpObjects("CelestialObject");
		}

		[Test]
		public void Save()
		{
			var asteroidBelt = AsteroidBelt.Build(null);
			Assert.That(asteroidBelt, Is.InstanceOf<AsteroidBelt>());
			var state = asteroidBelt.Save();

			Assert.That(state.AsteroidBelt, Is.Not.Null);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(AsteroidBelt);
			var state = Database.UseOnceTo().GetById<CelestialObjectState>(1L);
			Assert.That(state, Is.Not.Null);

			var celestialObject = state.Build(null);
			Assert.That(celestialObject, Is.InstanceOf<AsteroidBelt>());

			var asteroidBelt = (AsteroidBelt) celestialObject;
			Assert.That(asteroidBelt.Richness, Is.EqualTo(AsteroidBelt.AsteroidBelt.Richness));
		}
	}
}