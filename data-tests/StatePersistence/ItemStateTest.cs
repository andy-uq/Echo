using System;
using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
{
	[TestFixture]
	public class ItemStateTest : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
			public ItemState Value { get; set; }

			public WrappedObjectState(ItemState value)
			{
				Value = value;
			}
		}

		public ItemState Item
		{
			get { return Items.Item.Builder.Save(new Item(Universe.Item, quantity:10)); }
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
			var item = Echo.Items.Item.Builder.Build(Item).Build(idResolver);

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