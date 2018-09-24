using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Exceptions;
using Echo.Items;

namespace Echo
{
	public class PackerResult
	{
		public PackerResult(IObject item = null)
		{
			Item = item;
			Success = (item != null);
		}

		public IObject Item { get; }
		public bool Success { get; }
	}

	public interface IItemPacker
	{
		Type Type { get; }
		PackerResult Unpack(Item item);

		bool CanPack<T>(T item) where T : IObject;
		Item Pack(IObject item);
	}

	public class ItemPacker
	{
		private readonly Dictionary<Type, IItemPacker> _itemPackers;

		public ItemPacker(IEnumerable<IItemPacker> itemPackers)
		{
			_itemPackers = itemPackers.ToDictionary(k => k.Type);
		}

		public T Unpack<T>(Item item) where T : IObject
		{
			if (item.Quantity != 0)
			{
				var itemPacker = GetItemPacker<T>();
				var result = itemPacker.Unpack(item);
				if (result.Success)
				{
					item.Quantity--;
					return (T) result.Item;
				}
			}

			return default;
		}

		public bool CanPack<T>(T item) where T : IObject
		{
			var itemPacker = GetItemPacker<T>();
			return itemPacker.CanPack(item);
		}

		public Item Pack<T>(T item) where T : IObject
		{
			var itemPacker = GetItemPacker<T>();
			return itemPacker.Pack(item);
		}
		
		private IItemPacker GetItemPacker<T>() where T : IObject
		{
			if (_itemPackers.TryGetValue(typeof(T), out var builder))
				return builder;

			throw new ItemNotFoundException("Item unpacker", typeof(T).Name);
		}
	}
}