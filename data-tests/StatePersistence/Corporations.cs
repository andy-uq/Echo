using System;
using Echo.Builders;
using Echo.Corporations;
using Echo.State;
using Echo.Tests;
using Echo.Tests.Mocks;
using Echo.Tests.StatePersistence;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
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

		public CorporationState MSCorp
		{
			get { return Universe.MSCorp; }
		}

		[Test]
		public void Persist()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(MSCorp));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var builder = Corporation.Builder.Build(MSCorp);
			
			var corp = builder.Materialise();
			Assert.That(corp, Is.InstanceOf<Corporation>());
			var state = corp.Save();

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(MSCorp);
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
			}
		}

	}
}