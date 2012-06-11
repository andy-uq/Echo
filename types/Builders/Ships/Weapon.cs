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
				};
			}

			public static Weapon Build(WeaponState state)
			{
				if ( state == null )
				{
					return null;
				}

				var weapon = new Weapon
				{
					Id = state.ObjectId,
					Name = state.Name,
				};

				return weapon;
			}
		}
	}
}