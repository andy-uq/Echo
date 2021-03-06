using System;
using Echo.Builders;
using Echo.Corporations;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Corporations : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
			public CorporationState Value { get; set; }

			public WrappedObjectState(CorporationState value)
			{
				Value = value;
			}
		}

		public CorporationState Corp => Universe.MSCorp;

		[Test]
		public void Persist()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(Corp));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var builder = Corporation.Builder.Build(Corp);
			
			var corp = builder.Materialise();
			Assert.That(corp, Is.InstanceOf<Corporation>());
			var state = corp.Save();

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(Corp);
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

				var builder = Corporation.Builder.Build(state);
				
				var corporation = builder.Materialise();
				Assert.That(corporation, Is.InstanceOf<Corporation>());
				Assert.That(corporation.Employees, Has.All.Property("Corporation").EqualTo(corporation));
			}
		}

	}
}