using System;
using Echo.Ships;

namespace Echo
{
	public class TaskResult
	{
		public string Task { get; set; }

		public bool Success { get; set; }
		public string ErrorCode { get; set; }
		public object ErrorParams { get; set; }
	}

	public class WeaponAttackResult
	{
		public Ship Ship { get; set; }
		public Ship Target { get; set; }

		public bool Hit { get; set; }
		public Damage ArmourDamage { get; set; }
		public Damage HullDamage { get; set; }
	}
}