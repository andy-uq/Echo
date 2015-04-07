using System;
using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
{
	[TestFixture]
	public class BluePrintStateTest : StateTest
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
			get { return Items.Item.Builder.Save(new Item(Universe.BluePrint)); }
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
			var idResolver = new IdResolutionContext(new IObject[] { Universe.BluePrint, Universe.Weapon });
			var item = Echo.Items.Item.Builder.Build(Item).Build(idResolver);

			Assert.That(item.ObjectType, Is.EqualTo(ObjectType.Item));
			Assert.That(item.ItemInfo, Is.InstanceOf<BluePrintInfo>());

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