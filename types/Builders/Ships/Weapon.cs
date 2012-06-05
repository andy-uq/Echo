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
					Id = weapon.Id,
					Name = weapon.Name,
					LocalCoordinates = weapon.Position.LocalCoordinates
				};
			}

			public static Weapon Build(ILocation location, WeaponState state)
			{
				if ( state == null )
				{
					return null;
				}

				var weapon = new Weapon
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates)
				};

				return weapon;
			}
		}
	}
}