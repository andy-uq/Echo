using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.Data.Tests;
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
			public string Id { get; set; }
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
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(Moon));
				session.SaveChanges();
			}

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

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(Moon);
			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();
			}

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);

				var moon = CelestialObject.Builder.For(state).Build(null, state).Materialise();
				Assert.That(moon, Is.InstanceOf<Moon>());
			}
		}
	}
}