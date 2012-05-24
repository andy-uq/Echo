using Echo.Celestial;
using Echo.State;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Planets : StateTest
	{
		private CelestialObjectState Earth
		{
			get { return Universe.Earth; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(Earth);
			DumpObjects("CelestialObject");
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Earth);
			var state = Database.UseOnceTo().GetById<CelestialObjectState>(1L);
			Assert.That(state, Is.Not.Null);

			var earth = CelestialObject.Builder.For(state).Build(new Universe(), state);
			Assert.That(earth, Is.InstanceOf<Planet>());
		}
	}
}