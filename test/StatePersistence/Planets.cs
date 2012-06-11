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
		class WrappedObjectState
		{
			public Guid Id { get; set; }
			public State.CelestialObjectState Value { get; set; }

			public WrappedObjectState(State.CelestialObjectState value)
			{
				Value = value;
			}
		}
		
		private CelestialObjectState Earth
		{
			get { return Universe.Earth; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(new WrappedObjectState(Earth));
			DumpObjects("WrappedObject");
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
			var wrapped = new WrappedObjectState(Earth);
			Database.UseOnceTo().Insert(wrapped);
			var state = Database.UseOnceTo().GetById<WrappedObjectState>(wrapped.Id).Value;
			Assert.That(state, Is.Not.Null);

			var earth = CelestialObject.Builder.For(state).Build(null, state).Materialise();
			Assert.That(earth, Is.InstanceOf<Planet>());
		}
	}
}