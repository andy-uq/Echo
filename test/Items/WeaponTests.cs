using Echo.Ships;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class WeaponTests
	{
		private WeaponInfo _w1;

		[SetUp]
		public void SetUp()
		{
			_w1 = new WeaponInfo { MinimumDamage = 0, MaximumDamage = 100, Speed = 1d };
		}

		[Test]
		public void ObjectTypeIsWeapon()
		{
			var weapon = new Weapon();
			Assert.That(weapon.ObjectType, Is.EqualTo(ObjectType.Weapon));
		}

		[Test]
		public void DamagePerSecondCalculations()
		{
			Assert.That(_w1.DamagePerSecond, Is.EqualTo(50d));
		}

		[Test]
		public void AdvanceHardPointAttackCounter()
		{
			var hp = new HardPoint(null, HardPointPosition.Top);
			hp.EquipWeapon(new Weapon { WeaponInfo = _w1 });

			int attacks = GetAttackCount(hp, 10, 1d);
			Assert.That(attacks, Is.EqualTo(10));

			attacks = GetAttackCount(hp, 10, 1.25);
			Assert.That(attacks, Is.EqualTo(12));

			attacks = GetAttackCount(hp, 10, 0.7d);
			Assert.That(attacks, Is.EqualTo(6));
		}

		private int GetAttackCount(HardPoint hp, int numberOfRounds, double speed)
		{
			int attack = 0;

			hp.AttackCounter = 0;
			_w1.Speed = speed;

			for (int i = 0; i < numberOfRounds; i++)
			{
				hp.AttackCounter += hp.Weapon.WeaponInfo.Speed;
				while (hp.AttackCounter >= 1d)
				{
					attack++;
					hp.AttackCounter -= 1;
				}
			}

			return attack;
		}
	}
}