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

		public IObject Unpack(Item item)
		{
			var weaponInfo = (WeaponInfo) item.ItemInfo;
			return Weapon.Builder.Build(_idGenerator, weaponInfo);
		}

		public bool CanPack<T>(T item) where T : IObject
		{
			return false;
		}

		public Item Pack(IObject item)
		{
			throw new NotImplementedException();
		}
	}
}