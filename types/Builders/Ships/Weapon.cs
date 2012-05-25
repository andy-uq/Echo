using Echo.State;

namespace Echo.Ships
{
	partial class Weapon
	{
		public class Builder
		{
			public Weapon Build(ILocation location, WeaponState state)
			{
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