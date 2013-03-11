using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class ItemCollectionTests
	{
		private Item Item(uint quantity = 1)
		{
			var itemInfo = new ItemInfo(ItemCode.Veldnium);
			return new Item(itemInfo, quantity);
		}

		[Test]
		public void AddToLocalAddToParent()
		{
			var global = new ItemCollection();
			var remote = new ItemCollection(global);
			var local = new ItemCollection(global);

			var item = Item();
			local.Add(item);

			Assert.That(remote, Is.Empty);
			Assert.That(local, Contains.Item(item));
			Assert.That(global, Contains.Item(item));

			Assert.That(global.ItemCount, Is.EqualTo(1));
			Assert.That(local.ItemCount, Is.EqualTo(1));
		}

		[Test]
		public void AddToLocalAndRemoteAddToParent()
		{
			var global = new ItemCollection();
			var remote = new ItemCollection(global);
			var local = new ItemCollection(global);

			var item = Item();
			local.Add(item);
			remote.Add(item);

			Assert.That(global.ItemCount, Is.EqualTo(2));
			Assert.That(remote.ItemCount, Is.EqualTo(1));
			Assert.That(local.ItemCount, Is.EqualTo(1));
		}

		[Test]
		public void RemoveFromLocalRemoveFromParent()
		{
			var global = new ItemCollection();
			var local = new ItemCollection(global);

			local.Add(Item());
			local.Remove(Item());
			
			Assert.That(local, Is.Empty);
			Assert.That(global, Is.Empty);
		}
	}
}