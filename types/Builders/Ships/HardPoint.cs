using Echo.Builders;
using Echo.State;

namespace Echo.Ships
{
	partial class HardPoint
	{
		public static class Builder
		{
			public static HardPointState Save(HardPoint hardPoint)
			{
				return new HardPointState
				{
					Position = hardPoint.Position,
					Orientation = hardPoint.Orientation,
					Speed = hardPoint.Speed,
					Weapon = hardPoint.Weapon.Save()
				};
			}

			public static HardPoint Build(Ship ship, HardPointState state)
			{
				return new HardPoint(ship, state.Position)
				{
					Orientation = state.Orientation,
					Speed = state.Speed,
					Weapon = Weapon.Builder.Build(state.Weapon)
				};
			}
		}
	}
}