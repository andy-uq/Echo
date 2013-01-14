using System;
using System.Linq;
using Echo.Builder;
using Echo.Items;
using Echo.State;
using Echo.Tests.StatePersistence;
using NUnit.Framework;
using SisoDb.Serialization;

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
			get { return new ItemState {Code = ItemCode.MissileLauncher, Quantity = 10}; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(new WrappedObjectState(Item));
			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var idResolver = new IdResolutionContext(new[] { Universe.Weapon });
			var item = Echo.Items.Item.Builder.Build(Item, idResolver);

			Assert.That(item.ObjectType, Is.EqualTo(ObjectType.Item));

			var state = Echo.Items.Item.Builder.Save(item);
			Console.WriteLine(state.SerializeAndFormat());
		}

		[Test]
		public void Deserialise()
		{
			
		}
	}
}