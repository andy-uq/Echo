using System;
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

		public ItemCollection(Corporation owner = null, ILocation location = null, IEnumerable<Item> initialContents = null)
		{
			_owner = owner;
			_location = location;

			_storage = new Dictionary<ItemCode, Item>();
			_subCollections = new List<ItemCollection>();

			if ( initialContents == null )
				return;

			foreach (var item in initialContents)
			{
				Add(item);
			}
		}

		public ItemCollection(ItemCollection parent, ILocation location = null)
			: this(location: location)
		{
			if (parent == null) 
				return;

			_parent = parent;
			parent._subCollections.Add(this);
		}

		public ItemCollection(ItemCollection parent, IEnumerable<Item> initialContents)
			: this(parent)
		{
			foreach (var item in initialContents)
			{
				Add(item);
			}
		}

		public Corporation Owner => _owner ?? _parent?._owner;
		public ILocation Location => _location ?? _parent?._location;

		private IEnumerable<Item> Items => _storage.Values.Concat(_subCollections.SelectMany(x => x.Items));

		public IEnumerator<Item> GetEnumerator() => Items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(Item item)
		{
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (_storage.TryGetValue(item.ItemInfo.Code, out var currentItem))
			{
				currentItem.Quantity += item.Quantity;
				return;
			}

			_storage.Add(item.ItemInfo.Code, item);
			item.Owner = Owner;
			item.Location = Location;
		}

		public void Clear()
		{
			_storage.Clear();
		}

		public bool Contains(Item item)
		{
			if (item == null) throw new ArgumentNullException(nameof(item));

			var itemCode = item.ItemInfo.Code;
			var quantity = item.Quantity;

			return Contains(itemCode, quantity);
		}

		public bool Contains(ItemCode itemCode, uint quantity)
		{
			if (_storage.TryGetValue(itemCode, out var currentItem))
			{
				return currentItem.Quantity >= quantity;
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
			var itemCode = item.ItemInfo.Code;
			var quantity = item.Quantity;

			return Remove(itemCode, quantity);
		}

		private bool Remove(ItemCode itemCode, uint quantity)
		{
			if (_storage.TryGetValue(itemCode, out var currentItem))
			{
				if (currentItem.Quantity < quantity)
				{
					return false;
				}

				if (currentItem.Quantity == quantity)
				{
					_storage.Remove(itemCode);
				}
				else
				{
					currentItem.Quantity -= quantity;
				}

				return true;
			}

			return false;
		}

		public void Remove(IEnumerable<ItemState> materials)
		{
			var query = materials.GroupBy(m => m.Code, (key, g) => new { key, quantity = (uint) g.Sum(m => m.Quantity)});
			foreach (var item in query)
			{
				Remove(item.key, item.quantity);
			}
		}

		public int Count => _storage.Count;

		public bool IsReadOnly => false;

		public long	ItemCount => Items.Sum(i => i.Quantity);

		public bool IsEmpty => _storage.Count == 0;

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

		private uint GetQuantity(ItemCode code) => _storage.TryGetValue(code, out var item) ? item.Quantity : 0;
	}
}