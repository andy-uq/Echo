using System;
using Echo.Ships;
using Echo.State;

namespace Echo.Items.Packing
{
	public class WeaponPacker : IItemPacker
	{
		private readonly IIdGenerator _idGenerator;

		public WeaponPacker(IIdGenerator idGenerator)
		{
			_idGenerator = idGenerator;
		}

		public Type Type
		{
			get { return typeof(Weapon); }
		}

		public PackerResult Unpack(Item item)
		{
			var weaponInfo = (WeaponInfo) item.ItemInfo;
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