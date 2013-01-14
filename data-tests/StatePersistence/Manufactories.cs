using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.Data.Tests;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Manufactories : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
			public StructureState Value { get; set; }

			public WrappedObjectState(StructureState value)
			{
				Value = value;
			}
		}
		
		private StructureState Manufactory
		{
			get { return Universe.Manufactory; }
		}

		[Test]
		public void Persist()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(Manufactory));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var structure = Structure.Builder.For(Manufactory).Build(new Moon { Id = Universe.Moon.ObjectId }, Manufactory).Materialise();
			var state = structure.Save();

			Assert.That(state.Manufactory, Is.Not.Null);

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(Manufactory);

			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();
			}

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);
				Assert.That(state.Manufactory, Is.Not.Null);
				Assert.That(state.TradingStation, Is.Null);

				var structure = Structure.Builder.For(Manufactory).Build(null, Manufactory).Materialise();
				Assert.That(structure, Is.InstanceOf<Manufactory>());
			}
		}
	}
}