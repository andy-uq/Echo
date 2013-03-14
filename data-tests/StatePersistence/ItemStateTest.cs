using System;
using System.Linq;
using Echo.Builder;
using Echo.Items;
using Echo.State;
using Echo.Tests.StatePersistence;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
{
	[TestFixture]
	public class ItemStateTest : StateTest
	{
		public class WrappedObjectState
		{
			public Guid Id { get; set; }
			public ItemState Value { get; set; }

			public WrappedObjectState(ItemState value)
			{
				Value = value;
			}
		}

		public ItemState Item
		{
			get { return new ItemState {Code = Universe.Item.Code, Quantity = 10}; }
		}

		[Test]
		public void Persist()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(Item));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var idResolver = new IdResolutionContext(new[] { Universe.Item });
			var item = Echo.Items.Item.Builder.Build(Item, idResolver);

			Assert.That(item.ObjectType, Is.EqualTo(ObjectType.Item));

			var state = Echo.Items.Item.Builder.Save(item);
			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			
		}
	}
}