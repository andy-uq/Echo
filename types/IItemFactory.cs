using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Exceptions;
using Echo.Items;
using Echo.State;

namespace Echo
{
	public interface IItemFactory
	{
		Item Build(ItemCode itemCode, uint quantity);
		T Unpack<T>(Item item) where T : IObject;

		bool CanPack<T>(T item) where T : IObject;
		Item Pack<T>(T item) where T : IObject;
	}

	public interface IItemPacker
	{
		Type Type { get; }
		IObject Unpack(Item item);

		bool CanPack<T>(T item) where T : IObject;
		Item Pack(IObject item);
	}

	public class ItemFactory : IItemFactory
	{
		private readonly Dictionary<Type, IItemPacker> _itemPackers;
		private readonly IIdResolver _resolver;

		public ItemFactory(IIdResolver resolver, IEnumerable<IItemPacker> itemPackers)
		{
			_resolver = resolver;
			_itemPackers = itemPackers.ToDictionary(k => k.Type);
		}

		#region IItemFactory Members

		public Item Build(ItemCode itemCode, uint quantity)
		{
			var item = new ItemState {Code = itemCode, Quantity = quantity};
			return Item.Builder.Build(item, _resolver);
		}

		public T Unpack<T>(Item item) where T : IObject
		{
			IItemPacker itemPacker = GetItemPacker<T>();
			return (T) itemPacker.Unpack(item);
		}

		public bool CanPack<T>(T item) where T : IObject
		{
			IItemPacker itemPacker = GetItemPacker<T>();
			return itemPacker.CanPack(item);
		}

		public Item Pack<T>(T item) where T : IObject
		{
			IItemPacker itemPacker = GetItemPacker<T>();
			return itemPacker.Pack(item);
		}

		#endregion

		private IItemPacker GetItemPacker<T>() where T : IObject
		{
			IItemPacker builder;
			if (_itemPackers.TryGetValue(typeof (T), out builder))
				return builder;

			throw new ItemNotFoundException("Item unpacker", typeof (T).Name);
		}
	}
}