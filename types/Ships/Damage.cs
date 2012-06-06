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

		double IStatisticDelta<double>.Value
		{
			get { return -Value; }
		}
	}
}