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
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(AsteroidBelt);
			var state = Database.UseOnceTo().GetById<CelestialObjectState>(1L);
			Assert.That(state, Is.Not.Null);

			var celestialObject = CelestialObject.Builder.For(state).Build(new Universe(), state);
			Assert.That(celestialObject, Is.InstanceOf<AsteroidBelt>());

			var asteroidBelt = (AsteroidBelt) celestialObject;
			Assert.That(asteroidBelt.Richness, Is.EqualTo(AsteroidBelt.AsteroidBelt.Richness));
		}
	}
}