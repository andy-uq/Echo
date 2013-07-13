using System.Linq;
using Echo.Corporations;
using Echo.Items;
using Echo.Structures;
using NUnit.Framework;
using test.common;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ItemCollectionTests
	{
		private readonly ItemFactory _itemFactory = new ItemFactory(new TestIdResolver());
		private readonly Corporation _owner = new Corporation();
		private readonly Structure _manufactory = new Manufactory();

		[Test]
		public void AddItem()
		{
			var collection = new ItemCollection();
			collection.Add(_itemFactory.Build(ItemCode.LightFrigate, 1));

			Assert.That(collection.Count, Is.EqualTo(1));
		}

		[Test]
		public void RemoveItem()
		{
			var collection = new ItemCollection();
			collection.Add(_itemFactory.Build(ItemCode.LightFrigate, 1));
			collection.Remove(_itemFactory.Build(ItemCode.LightFrigate, 1));

			Assert.That(collection.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveMultipleItems()
		{
			var collection = new ItemCollection();
			collection.Add(_itemFactory.Build(ItemCode.LightFrigate, 10));
			collection.Remove(_itemFactory.Build(ItemCode.LightFrigate, 5));

			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.ItemCount, Is.EqualTo(5));
		}

		[Test]
		public void CanRemoveItemThatDoesNotExist()
		{
			var collection = new ItemCollection();
			collection.Add(_itemFactory.Build(ItemCode.LightFrigate, 10));

			bool canRemove = collection.Remove(_itemFactory.Build(ItemCode.MiningLaser, 1));
			Assert.That(canRemove, Is.False);

			canRemove = collection.Remove(_itemFactory.Build(ItemCode.LightFrigate, 20));
			Assert.That(canRemove, Is.False);

			Assert.That(collection.Contains(ItemCode.LightFrigate, 10));
		}

		[Test]
		public void Clear()
		{
			var collection = new ItemCollection();
			collection.Add(_itemFactory.Build(ItemCode.LightFrigate, 1));
			collection.Clear();

			Assert.That(collection.Count, Is.EqualTo(0));
		}

		[Test]
		public void CopyTo()
		{
			var item = _itemFactory.Build(ItemCode.LightFrigate, 1);

			var collection = new ItemCollection();
			collection.Add(item);

			var array = new Item[2];
			collection.CopyTo(array, 1);

			Assert.That(array, Is.EqualTo(new[] { null, item }));
		}

		[Test]
		public void AddItemTwice()
		{
			var collection = new ItemCollection
			{
				_itemFactory.Build(ItemCode.LightFrigate, 1),
				_itemFactory.Build(ItemCode.LightFrigate, 1)
			};

			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.ItemCount, Is.EqualTo(2));
		}

		[Test]
		public void Contains()
		{
			var collection = new ItemCollection
			{
				_itemFactory.Build(ItemCode.LightFrigate, 10)
			};

			Assert.That(collection.Contains(_itemFactory.Build(ItemCode.LightFrigate, 1)));
			Assert.That(collection.Contains(_itemFactory.Build(ItemCode.LightFrigate, 10)));
			Assert.That(collection.Contains(_itemFactory.Build(ItemCode.LightFrigate, 20)), Is.False);
			Assert.That(collection.Contains(_itemFactory.Build(ItemCode.MiningLaser, 1)), Is.False);
		}

		[Test]
		public void AddItemGetOwner()
		{
			var collection = new ItemCollection(_owner);
			collection.Add(_itemFactory.Build(ItemCode.LightFrigate, 1));

			Assert.That(collection.Count, Is.EqualTo(1));

			var item = collection.First();
			Assert.That(item.Owner, Is.EqualTo(_owner));
		}

		[Test]
		public void AddSubItemGetOwner()
		{
			var collection = new ItemCollection(_owner);
			var hangar = new ItemCollection(collection);

			hangar.Add(_itemFactory.Build(ItemCode.LightFrigate, 1));

			var item = hangar.First();
			Assert.That(item.Owner, Is.EqualTo(_owner));
		}

		[Test]
		public void AddSubItemWithLocation()
		{
			var collection = new ItemCollection(_owner);
			var hangar = new ItemCollection(collection, location:_manufactory);

			hangar.Add(_itemFactory.Build(ItemCode.LightFrigate, 1));

			var item = hangar.First();
			Assert.That(item.Location, Is.EqualTo(_manufactory));
		}
	}
}