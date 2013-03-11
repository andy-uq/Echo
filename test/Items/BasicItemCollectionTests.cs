using System.Linq;
using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class BasicItemCollectionTests
	{
		private Item Item(uint quantity=1)
		{
			var itemInfo = new ItemInfo(ItemCode.Veldnium);
			return new Item(itemInfo, quantity);
		}

		[Test]
		public void AddItem()
		{
			var collection = new ItemCollection { Item(quantity: 5) };
			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.ItemCount, Is.EqualTo(5));
		}

		[Test]
		public void AddItemOfSameType()
		{
			var collection = new ItemCollection();
			collection.Add(Item());
			collection.Add(Item(quantity:2));

			Assert.That(collection.ItemCount, Is.EqualTo(3));
		}

		[Test]
		public void GetItem()
		{
			
			var item = Item();
			var collection = new ItemCollection {item};

			Assert.That(collection.First(), Is.EqualTo(collection.First()));
		}

		[Test]
		public void CopyTo()
		{
			var item = Item();
			var collection = new ItemCollection {item};

			var items = new Item[1];
			collection.CopyTo(items, 0);

			Assert.That(items[0], Is.EqualTo(item));
		}

		[Test]
		public void RemoveItem()
		{
			
			var item = Item();
			var collection = new ItemCollection {item};
			collection.Remove(item);

			Assert.That(collection, Is.Empty);
		}

		[Test]
		public void RemoveItemOfSameType()
		{
			var collection = new ItemCollection {Item(quantity:5), Item(quantity:3)};
			collection.Remove(Item(quantity:4));

			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.ItemCount, Is.EqualTo(4));
		}

		[Test]
		public void CannotRemoveMoreThanWhatWeHave()
		{
			var collection = new ItemCollection {Item(quantity:5)};
			Assert.That(collection.Remove(Item(quantity:6)), Is.False);

			Assert.That(collection.Count, Is.EqualTo(1));
			Assert.That(collection.ItemCount, Is.EqualTo(5));
		}

		[Test]
		public void Count()
		{
			var item = Item();
			var collection = new ItemCollection {item};

			Assert.That(collection.Count, Is.EqualTo(1));
		}

		[Test]
		public void Clear()
		{
			var item = Item();
			var collection = new ItemCollection {item};
			collection.Clear();

			Assert.That(collection, Is.Empty);
		}

		[Test]
		public void ContainsItem()
		{
			var item = Item();
			var collection = new ItemCollection {item};
			
			Assert.IsTrue(collection.Contains(item));
		}

		[Test]
		public void ContainsItemWhenMore()
		{
			var collection = new ItemCollection {Item(quantity:5)};
			
			Assert.IsTrue(collection.Contains(Item(quantity:5)));
			Assert.IsTrue(collection.Contains(Item(quantity:4)));
		}

		[Test]
		public void ContainsItemWhenLess()
		{
			var collection = new ItemCollection {Item(quantity:5)};
			
			Assert.IsFalse(collection.Contains(Item(quantity:6)));
		}

		[Test]
		public void ContainsItemWhenNone()
		{
			var collection = new ItemCollection();
			Assert.IsFalse(collection.Contains(Item(quantity:6)));
		}
	}
}