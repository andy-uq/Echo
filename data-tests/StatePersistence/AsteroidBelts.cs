using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class AsteroidBelts : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
			public CelestialObjectState Value { get; set; }

			public WrappedObjectState(CelestialObjectState value)
			{
				Value = value;
			}
		}

		private CelestialObjectState AsteroidBelt => Universe.AsteroidBelt;

		[Test]
		public void Persist()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(AsteroidBelt));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var asteroidBelt = CelestialObject.Builder.For(AsteroidBelt).Build(null).Materialise();
			Assert.That(asteroidBelt, Is.InstanceOf<AsteroidBelt>());
			var state = asteroidBelt.Save();

			Assert.That(state.AsteroidBelt, Is.Not.Null);

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(AsteroidBelt);
			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);

				var celestialObject = CelestialObject.Builder.For(state).Build(null).Materialise();
				Assert.That(celestialObject, Is.InstanceOf<AsteroidBelt>());

				var asteroidBelt = (AsteroidBelt) celestialObject;
				Assert.That(asteroidBelt.Richness, Is.EqualTo(AsteroidBelt.AsteroidBelt.Richness));
			}
		}
	}
}