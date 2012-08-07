﻿namespace Echo.State
{
	public class WeaponInfo : ItemInfo
	{
		public DamageType DamageType { get; set; }
		public double Speed { get; set; }
		public int MinimumDamage { get; set; }
		public int MaximumDamage { get; set; }

		public double DamagePerSecond
		{
			get
			{
				var averageDmg = (MaximumDamage - MinimumDamage) / 2 + MinimumDamage;
				return averageDmg*Speed;
			}
		}
	}
}