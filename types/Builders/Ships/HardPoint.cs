using Echo.Builders;
using Echo.State;

namespace Echo.Ships
{
	partial class HardPoint
	{
		public class Builder
		{
			public HardPoint Build(Ship ship, HardPointState state)
			{
				return new HardPoint(ship, state.Position)
				{
					Orientation = state.Orientation,
					Speed = state.Speed,
					Weapon = state.Weapon.Build(ship)
				};
			}
		}
	}
}