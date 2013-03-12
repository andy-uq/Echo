using System;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.Corporations;
using Echo.State;
using Echo.Structures;
using Echo.Tests;
using Echo.Tests.Mocks;
using Echo.Tests.StatePersistence;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
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
			var builder = Structure.Builder.For(Manufactory)
				.Build(new Moon {Id = Universe.Moon.ObjectId});

			builder.Add(Corporation.Builder.Build(Universe.MSCorp));
			
			builder.RegisterTestSkills();

			var structure = builder.Materialise();
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
				
				var builder = Structure.Builder.For(Manufactory).Build(location: null);
				builder.Add(Corporation.Builder.Build(Universe.MSCorp));
				builder.RegisterTestSkills();

				var structure = builder.Materialise();

				Assert.That(structure, Is.InstanceOf<Manufactory>());
				Assert.That(structure.Owner, Is.Not.Null);
				Assert.That(structure.Owner.Id, Is.EqualTo(Universe.MSCorp.ObjectId));
			}
		}
	}
}