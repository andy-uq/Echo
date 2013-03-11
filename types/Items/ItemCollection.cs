using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Corporations;
using Echo.State;

namespace Echo.Items
{
	public class ItemCollection : ICollection<Item>
	{
		private readonly ItemCollection _parent;
		private readonly Corporation _owner;
		private readonly ILocation _location;

		private readonly Dictionary<ItemCode, Item> _storage;
		private readonly List<ItemCollection> _subCollections;

		public ItemCollection()
		{
			_storage = new Dictionary<ItemCode, Item>();
			_subCollections = new List<ItemCollection>();
		}

		public ItemCollection(ItemCollection parent) : this()
		{
			if (parent != null)
			{
				_parent = parent;
				parent._subCollections.Add(this);
			}
		}

		public ItemCollection(IEnumerable<Item> initialContents) : this(parent: null)
		{
			foreach (var item in initialContents)
			{
				Add(item);
			}
		}

		public Corporation Owner
		{
			get
			{
				if (_owner != null)
					return _owner;

				return _parent == null
				       	? null
				       	: _parent._owner;
			}
		}

		public ILocation Location
		{
			get
			{
				if (_location != null)
					return _location;

				return _parent == null
				       	? null
				       	: _parent._location;
			}
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

		public bool Contains(ICollection<ItemState> items)
		{
			var hasItems =
				(
					from neededItem in items
					select new
					{
						required = neededItem.Quantity,
						count = GetQuantity(neededItem.Code)
					}
				).All(i => i.required <= i.count);

			return hasItems || _subCollections.Any(x => x.Contains(items));
		}

		private uint GetQuantity(ItemCode code)
		{
			Item item;
			return _storage.TryGetValue(code, out item) ? item.Quantity : 0;
		}
	}
}