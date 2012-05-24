using Echo.Celestial;
using Echo.State;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Moons : StateTest
	{
		private CelestialObjectState Moon
		{
			get { return Universe.Moon; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(Moon);
			DumpObjects("CelestialObject");
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Moon);
			var state = Database.UseOnceTo().GetById<CelestialObjectState>(1L);
			Assert.That(state, Is.Not.Null);

			var moon = CelestialObject.Builder.For(state).Build(new Universe(), state);
			Assert.That(moon, Is.InstanceOf<Moon>());
		}
	}
}