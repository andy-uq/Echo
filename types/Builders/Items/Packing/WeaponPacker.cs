using System;
using Echo.Ships;
using Echo.State;

namespace Echo.Items.Packing
{
	public class WeaponPacker : IItemPacker
	{
		private readonly IIdGenerator _idGenerator;
		private readonly IItemFactory _itemFactory;

		public WeaponPacker(IIdGenerator idGenerator, IItemFactory itemFactory)
		{
			_idGenerator = idGenerator;
			_itemFactory = itemFactory;
		}

		public Type Type
		{
			get { return typeof(Weapon); }
		}

		public PackerResult Unpack(Item item)
		{
			var weaponInfo = _itemFactory.ToItemInfo<WeaponInfo>(ItemType.ShipWeapons, item.ItemInfo.Code);
			var weapon = Weapon.Builder.Build(_idGenerator, weaponInfo);
			
			return new PackerResult(weapon);
		}

		public bool CanPack<T>(T item) where T : IObject
		{
			return item is Weapon;
		}

		public Item Pack(IObject item)
		{
			var weapon = item as Weapon;
			return weapon == null 
				? null 
				: new Item(weapon.WeaponInfo);
		}
	}
}