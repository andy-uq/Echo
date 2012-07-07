using Echo.Ships;

namespace Echo.State
{
	public class HardPointState
	{
		public HardPointPosition Position { get; set; }
		public double Speed { get; set; }
		public double AttackCounter { get; set; }
		public Vector Orientation { get; set; }

		public WeaponState Weapon { get; set; }
	}
}