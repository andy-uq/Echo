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
		class WrappedObjectState
		{
			public Guid Id { get; set; }
			public State.CelestialObjectState Value { get; set; }

			public WrappedObjectState(State.CelestialObjectState value)
			{
				Value = value;
			}
		}
		
		private CelestialObjectState Moon
		{
			get { return Universe.Moon; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(new WrappedObjectState(Moon));
			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var moon = CelestialObject.Builder.For(Moon).Build(new Planet { Id = Universe.Earth.ObjectId }, Moon).Materialise();
			Assert.That(moon, Is.InstanceOf<Moon>());

			var state = moon.Save();

			Assert.That(state.Orbits, Is.Not.Null);
			Assert.That(state.Orbits.Value.Id, Is.EqualTo(Universe.Earth.ObjectId));

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapper = new WrappedObjectState(Moon);
			Database.UseOnceTo().Insert(wrapper);
			var state = Database.UseOnceTo().GetById<WrappedObjectState>(wrapper.Id).Value;
			Assert.That(state, Is.Not.Null);

			var moon = CelestialObject.Builder.For(state).Build(null, state).Materialise();
			Assert.That(moon, Is.InstanceOf<Moon>());
		}
	}
}