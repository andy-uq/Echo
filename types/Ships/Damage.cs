using Echo.Statistics;

namespace Echo.Ships
{
	public class Damage : IStatisticDelta<double>
	{
		public DamageType DamageType { get; set; }
		public double Value { get; set; }

		public Damage(DamageType damageType)
		{
			DamageType = damageType;
		}

		double IStatisticDelta<double>.Value => -Value;

		public static Damage operator +(Damage lValue, Damage rValue)
		{
			if (lValue == null || rValue == null)
				return lValue ?? rValue;

			var damageType = lValue.DamageType;
			if (rValue.DamageType != damageType)
				damageType = DamageType.Combined;

			return new Damage(damageType) { Value = lValue.Value + rValue.Value };
		}
	}
}