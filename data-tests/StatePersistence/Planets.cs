using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.Data.Tests;
using Echo.Data.Tests.StatePersistence;
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
			public string Id { get; set; }
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
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(Earth));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var planet = CelestialObject.Builder.For(Earth).Build(new Universe()).Materialise();
			Assert.That(planet, Is.InstanceOf<Planet>());

			var state = planet.Save();

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(Earth);
			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();
			}

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);

				var earth = CelestialObject.Builder.For(state).Build(null).Materialise();
				Assert.That(earth, Is.InstanceOf<Planet>());
				Assert.That(earth.Position.LocalCoordinates, Is.Not.EqualTo(Vector.Zero));
			}
		}
	}
}