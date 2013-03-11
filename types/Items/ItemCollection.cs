using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Echo.Items
{
	public class ItemCollection : ICollection<Item>
	{
		private readonly Dictionary<ItemCode, Item> _storage;
		private readonly List<ItemCollection> _subCollections;

		public ItemCollection()
		{
			_storage = new Dictionary<ItemCode, Item>();
			_subCollections = new List<ItemCollection>();
		}

		public ItemCollection(ItemCollection parent) : this()
		{
			parent._subCollections.Add(this);
		}

		private IEnumerable<Item> Items
		{
			get { return _storage.Values.Concat(_subCollections.SelectMany(x => x.Items)); }
		}

		public IEnumerator<Item> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(Item item)
		{
			Item currentItem;
			if (_storage.TryGetValue(item.ItemInfo.Code, out currentItem))
			{
				currentItem.Quantity += item.Quantity;
				return;
			}

			_storage.Add(item.ItemInfo.Code, item);
		}

		public void Clear()
		{
			_storage.Clear();
		}

		public bool Contains(Item item)
		{
			Item currentItem;
			if ( _storage.TryGetValue(item.ItemInfo.Code, out currentItem) )
			{
				return currentItem.Quantity >= item.Quantity;
			}

			return false;
		}

		public void CopyTo(Item[] array, int arrayIndex)
		{
			var i = arrayIndex;
			foreach ( var item in Items )
			{
				array[i++] = item;
			}
		}

		public bool Remove(Item item)
		{
			Item currentItem;
			if (_storage.TryGetValue(item.ItemInfo.Code, out currentItem))
			{
				if (currentItem.Quantity < item.Quantity)
				{
					return false;
				}

				if (currentItem.Quantity == item.Quantity)
				{
					_storage.Remove(item.ItemInfo.Code);
				}
				else
				{
					currentItem.Quantity -= item.Quantity;
				}

				return true;
			}

			return false;
		}

		public int Count
		{
			get { return _storage.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public long	ItemCount
		{
			get { return Items.Sum(i => i.Quantity); }
		}
	}
}