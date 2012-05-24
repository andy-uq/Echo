using System;
using System.Collections.Generic;

using ShipStatisticValue = Echo.StatisticValue<Echo.ShipStatistic, double>;

namespace Echo.Ships
{
	public class ShipStatistics
	{
		private readonly Dictionary<ShipStatistic, ShipStatisticValue> stats;

		public ShipStatistics() : this(null)
		{
		}

		public ShipStatistics(ShipStatistics factoryStats)
		{
			this.stats = new Dictionary<ShipStatistic, ShipStatisticValue>();

			foreach ( ShipStatistic stat in Enum.GetValues(typeof(ShipStatistic)) )
			{
				var val = (factoryStats == null) ? new ShipStatisticValue(stat, 0) : factoryStats[stat];
				this.stats.Add(stat, val);
			}
		}

		public ShipStatisticValue ArmourStrength(DamageType damageType)
		{
			switch (damageType)
			{
				case DamageType.Ballistic:
					return this.stats[ShipStatistic.BallisticArmourStrength];

				case DamageType.Energy:
					return this.stats[ShipStatistic.EnergyArmourStrength];
                    
				case DamageType.Kinetic:
					return this.stats[ShipStatistic.KineticArmourStrength];
				
				case DamageType.Thermal:
					return this.stats[ShipStatistic.ThermalArmourStrength];
				
				default:
					throw new ArgumentOutOfRangeException("damageType");
			}
		}

		public ShipStatisticValue this[ShipStatistic stat]
		{
			get { return this.stats[stat]; }
		}
	}
}