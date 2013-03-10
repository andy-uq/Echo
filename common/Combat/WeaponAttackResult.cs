using Echo.Ships;

namespace Echo.Combat
{
	public class WeaponAttackResult
	{
		public Ship Ship { get; set; }
		public Ship Target { get; set; }

		public bool Hit { get; set; }
		public Damage ArmourDamage { get; set; }
		public Damage HullDamage { get; set; }

		public Damage TotalDamage { get { return ArmourDamage + HullDamage; } }
	}
}