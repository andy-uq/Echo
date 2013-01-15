using System;
using Echo.Ships;

namespace Echo
{
	public abstract class TaskResult
	{
		public string Task { get; set; }
		public bool Success { get; set; }
	}

	public interface ITaskResult
	{
		string Task { get; }

		bool Success { get; }
		string ErrorCode { get; }
		object ErrorParams { get; }
	}

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