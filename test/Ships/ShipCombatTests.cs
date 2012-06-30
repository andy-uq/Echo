using System.Collections.Generic;
using Echo.Ships;
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

		[SetUp]
		public void SetUp()
		{
			_random = new Mock<IRandom>(MockBehavior.Strict);
			_context = new Moq.Mock<IEchoContext>(MockBehavior.Strict);

			_context.SetupGet(x => x.Random).Returns(_random.Object);

			var ship = new Ship();
			var target = new Ship {Statistics = new ShipStatistics(Stats())};
			_weapon = new Weapon();
			_combat = new AttackShipCombat(_context.Object) { Ship = ship, Target = target };
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
			_random.Setup(x => x.GetNext()).Returns(0);
			var result = _combat.Fire(_weapon);
			Assert.That(result.Ship, Is.EqualTo(_combat.Ship));
			Assert.That(result.Target, Is.EqualTo(_combat.Target));
			Assert.That(result.Hit, Is.True);
		}

		[Test]
		public void DamageRoll()
		{
			_weapon.MinimumDamage = 50;
			_weapon.MaximumDamage = 100;

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
			_weapon.MinimumDamage = 50;
			_weapon.MaximumDamage = 100;
			
			_random.Setup(x => x.GetNext()).Returns(0);
			var result = _combat.Fire(_weapon);

			Assert.That(result.ArmourDamage.Value, Is.GreaterThan(0));
			Assert.That(result.HullDamage, Is.Null);
		}

		[Test]
		public void HullDamaged()
		{
			_weapon.MinimumDamage = 50;
			_weapon.MaximumDamage = 200;

			var rollIndex = 0;
			var rolls = new[] { 0d, 0.5d };

			_random.Setup(x => x.GetNext()).Returns(() => rolls[rollIndex++]);
			var result = _combat.Fire(_weapon);

			Assert.That(result.ArmourDamage.Value, Is.GreaterThan(0));
			Assert.That(result.HullDamage, Is.Not.Null);
		}
	}
}