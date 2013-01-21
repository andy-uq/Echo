using Echo.Ships;
using Echo.State;
using Echo.Statistics;
using NUnit.Framework;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class ShipShieldsRepair
	{
		private ShieldInfo _shieldInfo;
		private Ship _ship;

		[SetUp]
		public void SetUp()
		{
			_shieldInfo = new ShieldInfo() { Statistic = ShipStatistic.EnergyArmourStrength, RepairPerTick = 5d };
			_ship = new Ship()
			{
				Statistics =
					new ShipStatistics(new[]
					{
						new StatisticValue<ShipStatistic, double>(
							ShipStatistic.EnergyArmourStrength,
							value: 100d
							)
					})
			};
		}

		private void DamageShip(double delta)
		{
			_ship.Statistics.ArmourStrength(DamageType.Energy).Alter(new Damage(DamageType.Energy) { Value = delta });
		}

		[Test]
		public void SmallDamage()
		{
			for ( var i = 0; i < 5; i++ )
				DamageShip(1d);

			var repair = new ArmourRepair();
			repair.Repair(_ship, _shieldInfo);

			Assert.That(_ship.Statistics[ShipStatistic.EnergyArmourStrength].CurrentValue, Is.EqualTo(100d));
		}

		[Test]
		public void LargeDamage()
		{
			DamageShip(10d);

			var repair = new ArmourRepair();
			repair.Repair(_ship, _shieldInfo);

			Assert.That(_ship.Statistics[ShipStatistic.EnergyArmourStrength].CurrentValue, Is.EqualTo(95d));
		}

		[Test]
		public void ShieldRepair()
		{
			DamageShip(5d);

			var repair = new ArmourRepair();
			repair.Repair(_ship, _shieldInfo);

			Assert.That(_ship.Statistics[ShipStatistic.EnergyArmourStrength].CurrentValue, Is.EqualTo(100d));
		}

		[Test]
		public void UndamagedShipRepair()
		{
			var repair = new ArmourRepair();
			repair.Repair(_ship, _shieldInfo);

			Assert.That(_ship.Statistics[ShipStatistic.EnergyArmourStrength].CurrentValue, Is.EqualTo(100d));
		}
	}
}