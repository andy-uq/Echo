using Echo.Builder;
using Echo.Items;
using Echo.State;

namespace Echo
{
	public interface IIdGenerator
	{
		ulong NextId();
	}
}

namespace Echo.Ships
{
	partial class Weapon
	{
		public static class Builder
		{
			public static WeaponState Save(Weapon weapon)
			{
				return new WeaponState
				{
					ObjectId = weapon.Id,
					Name = weapon.Name,
					Code = weapon.WeaponInfo.Code,
				};
			}

			public static Weapon Build(IIdResolver resolver, WeaponState state)
			{
				if ( state == null )
				{
					return null;
				}

				var weaponInfo = resolver.GetById<WeaponInfo>(ItemType.ShipWeapons.ToId(state.Code));
				return Build(state.ObjectId, weaponInfo);
			}
			
			public static Weapon Build(IIdGenerator idGenerator, WeaponInfo weaponInfo)
			{
				var id = idGenerator.NextId();
				return Build(id, weaponInfo);
			}

			private static Weapon Build(ulong id, WeaponInfo weaponInfo)
			{
				return new Weapon
				{
					Id = id,
					WeaponInfo = weaponInfo,
				};
			}
		}
	}
}