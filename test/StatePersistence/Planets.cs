using System;
using Echo.Builders;
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
		public void Save()
		{
			var planet = CelestialObject.Builder.For(Earth).Build(new Universe(), Earth).Materialise();
			Assert.That(planet, Is.InstanceOf<Planet>());

			var state = planet.Save();

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Earth);
			var state = Database.UseOnceTo().GetById<CelestialObjectState>(Earth.Id);
			Assert.That(state, Is.Not.Null);

			var earth = CelestialObject.Builder.For(state).Build(null, state).Materialise();
			Assert.That(earth, Is.InstanceOf<Planet>());
		}
	}
}