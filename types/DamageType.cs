using System;
using Echo.Statistics;

namespace Echo
{
	public enum DamageType
	{
		Combined,
		Ballistic,
		Energy,
		Kinetic,
		Thermal		
	}

	public static class DamageTypeExtensions
	{
		public static ShipStatistic GetCorrespondingArmourStat(this DamageType damageType)
		{
			switch ( damageType )
			{
				case DamageType.Combined:
					return ShipStatistic.HullIntegrity;

				case DamageType.Ballistic:
					return ShipStatistic.BallisticArmourStrength;

				case DamageType.Energy:
					return ShipStatistic.EnergyArmourStrength;

				case DamageType.Kinetic:
					return ShipStatistic.KineticArmourStrength;

				case DamageType.Thermal:
					return ShipStatistic.ThermalArmourStrength;

				default:
					throw new ArgumentOutOfRangeException(nameof(damageType));
			}
		}
	}
}