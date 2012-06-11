using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class AsteroidBelts : StateTest
	{
		public class WrappedObjectState
		{
			public Guid Id { get; set; }
			public CelestialObjectState Value { get; set; }

			public WrappedObjectState(CelestialObjectState value)
			{
				Value = value;
			}
		}

		private State.CelestialObjectState AsteroidBelt
		{
			get { return Universe.AsteroidBelt; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(new WrappedObjectState(AsteroidBelt));
			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var asteroidBelt = CelestialObject.Builder.For(AsteroidBelt).Build(null, AsteroidBelt).Materialise();
			Assert.That(asteroidBelt, Is.InstanceOf<AsteroidBelt>());
			var state = asteroidBelt.Save();

			Assert.That(state.AsteroidBelt, Is.Not.Null);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(AsteroidBelt);
			Database.UseOnceTo().Insert(wrapped);

			var state = Database.UseOnceTo().GetById<WrappedObjectState>(wrapped.Id).Value;
			Assert.That(state, Is.Not.Null);

			var celestialObject = CelestialObject.Builder.For(state).Build(null, state).Materialise();
			Assert.That(celestialObject, Is.InstanceOf<AsteroidBelt>());

			var asteroidBelt = (AsteroidBelt) celestialObject;
			Assert.That(asteroidBelt.Richness, Is.EqualTo(AsteroidBelt.AsteroidBelt.Richness));
		}
	}
}