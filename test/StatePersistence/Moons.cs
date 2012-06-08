using System;
using Echo.Builders;
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
		public void Save()
		{
			var moon = CelestialObject.Builder.For(Moon).Build(null, Moon).Materialise();
			Assert.That(moon, Is.InstanceOf<Moon>());

			var state = moon.Save();

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Moon);
			var state = Database.UseOnceTo().GetById<CelestialObjectState>(Moon.Id);
			Assert.That(state, Is.Not.Null);

			var moon = CelestialObject.Builder.For(state).Build(null, state).Materialise();
			Assert.That(moon, Is.InstanceOf<Moon>());
		}
	}
}