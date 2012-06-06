using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Ships;
using Echo.Statistics;
using NUnit.Framework;
using ShipStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.ShipStatistic, double>;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class ShipStatisticTests
	{
		private ShipStatistics _shipStatistics;

		[SetUp]
		public void SetUp()
		{
			_shipStatistics = new ShipStatistics(StatFactory());
		}

		[Test]
		public void DefaultStats()
		{
			var empty = new ShipStatistics();
			var armours =
				(
					from DamageType d in Enum.GetValues(typeof(DamageType))
					select empty.ArmourStrength(d)
				);
		
			Assert.That(armours, Is.All.Matches<ShipStatisticValue>(x => Units.IsZero(x.Value)));
		}

		[Test]
		public void SetInitialValues()
		{
			var empty = new ShipStatistics();

			empty[ShipStatistic.HullIntegrity].SetValue(1000d);
			Assert.That(empty[ShipStatistic.HullIntegrity].Value, Is.EqualTo(1000d));
			Assert.That(empty[ShipStatistic.HullIntegrity].CurrentValue, Is.EqualTo(1000d));

			empty[ShipStatistic.HullIntegrity].SetValue(10000d, empty[ShipStatistic.HullIntegrity].CurrentValue);
			Assert.That(empty[ShipStatistic.HullIntegrity].Value, Is.EqualTo(10000d));
			Assert.That(empty[ShipStatistic.HullIntegrity].CurrentValue, Is.EqualTo(1000d));
		}

		[Test]
		public void Compare()
		{
			var ship1 = new ShipStatisticValue(ShipStatistic.Speed, 1000d);

			Assert.That(ship1 < 2000d);
			Assert.That(ship1 <= 1000d);
			Assert.That(ship1 > 500d);
			Assert.That(ship1 >= 1000d);

			Assert.That(((IComparable<double>)ship1).CompareTo(1000d), Is.EqualTo(0));
			Assert.That(((IComparable<double>)ship1).CompareTo(2000d), Is.EqualTo(-1));
			Assert.That(((IComparable<double>)ship1).CompareTo(500d), Is.EqualTo(1));
		}

		[Test]
		public void Armour()
		{
			var armours =
				(
					from DamageType d in Enum.GetValues(typeof(DamageType))
					select _shipStatistics.ArmourStrength(d)
				).ToArray();

			Assert.That(armours.All(value => !value.Buffs.Any()));
			Assert.That(armours.All(value => !value.Debuffs.Any()));
		}

		[Test]
		public void DebuffArmour()
		{
			var shipDamage = new Damage(DamageType.Ballistic) { Value = 50 };
			var ballistic = _shipStatistics.ArmourStrength(DamageType.Ballistic);
			ballistic.Alter(shipDamage);

			Assert.That(ballistic.Debuffs.Any(), Is.True);
			Assert.That(ballistic.Buffs.Any(), Is.False);
		}

		[Test]
		public void BuffArmour()
		{
			var armourPlating = new ArmourPlating(ShipStatistic.BallisticArmourStrength) { Value = 50 };
			var ballistic = _shipStatistics.ArmourStrength(DamageType.Ballistic);
			ballistic.Alter(armourPlating);

			Assert.That(ballistic.Buffs.Any(), Is.True);
			Assert.That(ballistic.Debuffs.Any(), Is.False);
		}

		[Test]
		public void HullIntegrity()
		{
			var x = new ShipStatistics(StatFactory());

			var hullIntegrity = x[ShipStatistic.HullIntegrity];
			Assert.That(hullIntegrity.Value, Is.EqualTo(1000));

			var shipDamage = new Damage(DamageType.Ballistic) { Value = 10 };
			hullIntegrity.Alter(shipDamage);

			Assert.That(hullIntegrity.Value, Is.EqualTo(1000));
			Assert.That(hullIntegrity.CurrentValue, Is.EqualTo(990));
			Assert.That(hullIntegrity.Debuffs.Any(), Is.True);
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