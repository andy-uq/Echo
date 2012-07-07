using Echo.Builder;
using Echo.Items;
using Echo.State;

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

				return new Weapon
				{
					Id = state.ObjectId,
					Name = state.Name,
					WeaponInfo = resolver.GetById<WeaponInfo>(state.Code.ToId()),
				};
			}
		}
	}
}