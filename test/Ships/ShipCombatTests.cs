using System.Collections.Generic;
using Echo.Ships;
using Echo.State;
using Echo.Statistics;
using Moq;
using NUnit.Framework;
using ShipStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.ShipStatistic, double>;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class ShipCombatTests
	{
		private AttackShipCombat _combat;
		private Weapon _weapon;
		private Mock<IEchoContext> _context;
		private Mock<IRandom> _random;
		private Ship _target;

		[SetUp]
		public void SetUp()
		{
			_random = new Mock<IRandom>(MockBehavior.Strict);
			_context = new Moq.Mock<IEchoContext>(MockBehavior.Strict);

			_context.SetupGet(x => x.Random).Returns(_random.Object);

			var ship = new Ship();
			_target = new Ship {Statistics = new ShipStatistics(Stats())};
			_weapon = new Weapon();
			_combat = new AttackShipCombat(_context.Object) { Ship = ship, Target = _target };
		}

		private IEnumerable<ShipStatisticValue> Stats()
		{
			yield return new ShipStatisticValue(ShipStatistic.EnergyArmourStrength, 100d);
			yield return new ShipStatisticValue(ShipStatistic.HullIntegrity, 100d);
		}

		[Test]
		public void Miss()
		{
			_random.Setup(x => x.GetNext()).Returns(1);
			var result = _combat.Fire(_weapon);
			Assert.That(result.Ship, Is.EqualTo(_combat.Ship));
			Assert.That(result.Target, Is.EqualTo(_combat.Target));
			Assert.That(result.Hit, Is.False);
		}

		[Test]
		public void Hit()
		{
			_weapon.WeaponInfo = new WeaponInfo { DamageType = DamageType.Energy, MinimumDamage = 50, MaximumDamage = 200 };

			_random.Setup(x => x.GetNext()).Returns(0);
			var result = _combat.Fire(_weapon);
			Assert.That(result.Ship, Is.EqualTo(_combat.Ship));
			Assert.That(result.Target, Is.EqualTo(_combat.Target));
			Assert.That(result.Hit, Is.True);
		}

		[Test]
		public void DamageRoll()
		{
			_weapon.WeaponInfo = new WeaponInfo { DamageType = DamageType.Energy, MinimumDamage = 50, MaximumDamage = 100 };

			var rollIndex = 0;
			var rolls = new[] {0d, 0.5d};

			_random.Setup(x => x.GetNext()).Returns(() => rolls[rollIndex++]);
			var result = _combat.Fire(_weapon);

			Assert.That(result.ArmourDamage.Value, Is.EqualTo(75d));
			Assert.That(result.HullDamage, Is.Null);
		}

		[Test]
		public void ArmourDamaged()
		{
			_weapon.WeaponInfo = new WeaponInfo { DamageType = DamageType.Energy, MinimumDamage = 50, MaximumDamage = 100 };
			
			_random.Setup(x => x.GetNext()).Returns(0);
			var result = _combat.Fire(_weapon);

			Assert.That(result.ArmourDamage.Value, Is.GreaterThan(0));
			Assert.That(result.HullDamage, Is.Null);

			var hullIntegrity = _target.Statistics[ShipStatistic.HullIntegrity];
			Assert.That(hullIntegrity.CurrentValue, Is.EqualTo(100d));
		}

		[Test]
		public void HullDamaged()
		{
			_weapon.WeaponInfo = new WeaponInfo { DamageType = DamageType.Energy, MinimumDamage = 50, MaximumDamage = 200 };

			var rollIndex = 0;
			var rolls = new[] { 0d, 0.5d };

			_random.Setup(x => x.GetNext()).Returns(() => rolls[rollIndex++]);
			var result = _combat.Fire(_weapon);

			Assert.That(result.ArmourDamage.Value, Is.GreaterThan(0));
			Assert.That(result.HullDamage, Is.Not.Null);

			var hullIntegrity = _target.Statistics[ShipStatistic.HullIntegrity];
			Assert.That(hullIntegrity.CurrentValue, Is.LessThanOrEqualTo(100d));
		}

		[Test]
		public void ShipDestroyed()
		{
			_weapon.WeaponInfo = new WeaponInfo { DamageType = DamageType.Energy, MinimumDamage = 50, MaximumDamage = 500 };

			var rollIndex = 0;
			var rolls = new[] { 0d, 0.5d };

			_random.Setup(x => x.GetNext()).Returns(() => rolls[rollIndex++]);
			var result = _combat.Fire(_weapon);

			Assert.That(result.ArmourDamage.Value, Is.GreaterThan(0));
			Assert.That(result.HullDamage, Is.Not.Null);

			var hullIntegrity = _target.Statistics[ShipStatistic.HullIntegrity];
			Assert.That(hullIntegrity.CurrentValue, Is.LessThanOrEqualTo(0d));
		}
	}
}