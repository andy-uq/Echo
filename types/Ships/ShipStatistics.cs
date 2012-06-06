using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Statistics;
using ShipStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.ShipStatistic, double>;

namespace Echo.Ships
{
	public class ShipStatistics : IEnumerable<ShipStatisticValue>
	{
		private readonly Dictionary<ShipStatistic, ShipStatisticValue> _stats;

		static ShipStatistics()
		{
			ShipStatisticValue.InitMath(Statistics.Math.Double);
		}

		public ShipStatistics(IEnumerable<ShipStatisticValue> initialStats = null)
		{
			_stats = new Dictionary<ShipStatistic, ShipStatisticValue>();

			var factoryStats = (initialStats ?? Enumerable.Empty<ShipStatisticValue>()).ToDictionary(x => x.Stat);
			foreach (ShipStatistic stat in Enum.GetValues(typeof (ShipStatistic)))
			{
				ShipStatisticValue factoryValue;
				var value = new ShipStatisticValue(stat, 0);
				if (factoryStats.TryGetValue(stat, out factoryValue))
				{
					value = factoryValue;
				}

				_stats.Add(stat, value);
			}
		}

		public ShipStatisticValue this[ShipStatistic stat]
		{
			get { return _stats[stat]; }
		}

		public ShipStatisticValue ArmourStrength(DamageType damageType)
		{
			switch (damageType)
			{
				case DamageType.Ballistic:
					return _stats[ShipStatistic.BallisticArmourStrength];

				case DamageType.Energy:
					return _stats[ShipStatistic.EnergyArmourStrength];

				case DamageType.Kinetic:
					return _stats[ShipStatistic.KineticArmourStrength];

				case DamageType.Thermal:
					return _stats[ShipStatistic.ThermalArmourStrength];

				default:
					throw new ArgumentOutOfRangeException("damageType");
			}
		}

		public IEnumerator<ShipStatisticValue> GetEnumerator()
		{
			return _stats.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}