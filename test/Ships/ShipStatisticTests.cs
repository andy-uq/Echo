using System.Collections.Generic;
using Echo.Ships;
using Echo.Statistics;
using NUnit.Framework;
using ShipStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.ShipStatistic, double>;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class ShipStatisticTests
	{
		[Test]
		public void Armour()
		{
			var x = new ShipStatistics(StatFactory());

			var ballistic = x.ArmourStrength(DamageType.Ballistic);
			ballistic.Alter(60);
			Assert.That(ballistic.IsBuffed, Is.True);
		}

		[Test]
		public void HullIntegrity()
		{
			var x = new ShipStatistics(StatFactory());

			var hullIntegrity = x[ShipStatistic.HullIntegrity];
			Assert.That(hullIntegrity.Value, Is.EqualTo(1000));

			hullIntegrity.Alter(10);
			Assert.That(hullIntegrity.Value, Is.EqualTo(1000));
			Assert.That(hullIntegrity.CurrentValue, Is.EqualTo(10));
			Assert.That(hullIntegrity.IsDebuffed, Is.True);

		}

		public IEnumerable<ShipStatisticValue> StatFactory()
		{
			yield return new ShipStatisticValue(ShipStatistic.HullIntegrity, 1000);
			yield return new ShipStatisticValue(ShipStatistic.Speed, 50d);
			yield return new ShipStatisticValue(ShipStatistic.BallisticArmourStrength, 50d);
			yield return new ShipStatisticValue(ShipStatistic.EnergyArmourStrength, 50d);
			yield return new ShipStatisticValue(ShipStatistic.KineticArmourStrength, 50d);
			yield return new ShipStatisticValue(ShipStatistic.ThermalArmourStrength, 50d);
		}
	}
}