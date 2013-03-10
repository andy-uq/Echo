using Echo.Combat;
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
							),
						new StatisticValue<ShipStatistic, double>(
							ShipStatistic.BallisticArmourStrength,
							value: 100d
							),
					})
			};
		}

		private void DamageShip(double delta, DamageType damageType = DamageType.Energy)
		{
			var armour = _ship.Statistics.ArmourStrength(damageType);
			
			delta = System.Math.Min(delta, armour.CurrentValue);
			
			var damage = new Damage(damageType) {Value = delta};
			armour.Alter(damage);
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
		public void ShieldRepairsCorrectDamage()
		{
			DamageShip(10d, DamageType.Ballistic);

			var repair = new ArmourRepair();
			repair.Repair(_ship, _shieldInfo);

			Assert.That(_ship.Statistics[ShipStatistic.EnergyArmourStrength].CurrentValue, Is.EqualTo(100d));
			Assert.That(_ship.Statistics[ShipStatistic.BallisticArmourStrength].CurrentValue, Is.EqualTo(90d));
		}

		[Test]
		public void LargeDamage()
		{
			DamageShip(10d, DamageType.Energy);

			var repair = new ArmourRepair();
			repair.Repair(_ship, _shieldInfo);

			Assert.That(_ship.Statistics[ShipStatistic.EnergyArmourStrength].CurrentValue, Is.EqualTo(95d));
		}

		[Test]
		public void ShieldRepair()
		{
			DamageShip(5d, DamageType.Energy);

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