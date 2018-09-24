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

		public ShipStatistics(IEnumerable<ShipStatisticValue> initialStats = null)
		{
			_stats = new Dictionary<ShipStatistic, ShipStatisticValue>();

			var factoryStats = (initialStats ?? Enumerable.Empty<ShipStatisticValue>()).ToDictionary(x => x.Stat);
			foreach (ShipStatistic stat in Enum.GetValues(typeof (ShipStatistic)))
			{
				var value = new ShipStatisticValue(stat, 0);
				if (factoryStats.TryGetValue(stat, out var factoryValue))
				{
					value = factoryValue;
				}

				_stats.Add(stat, value);
			}
		}

		public ShipStatisticValue this[ShipStatistic stat] => _stats[stat];

		public ShipStatisticValue ArmourStrength(DamageType damageType)
		{
			var armourType = damageType.GetCorrespondingArmourStat();
			return _stats[armourType];
		}

		public IEnumerator<ShipStatisticValue> GetEnumerator() => _stats.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}