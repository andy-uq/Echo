using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Statistics;
using ShipStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.ShipStatistic, double>;

namespace Echo.Ships
{
	public class ShipStatistics
	{
		private readonly Dictionary<ShipStatistic, ShipStatisticValue> _stats;

		public ShipStatistics()
			: this(null)
		{
		}

		public ShipStatistics(IEnumerable<ShipStatisticValue> initialStats)
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
	}
}