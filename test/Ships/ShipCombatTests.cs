using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Builder;
using Echo.Items;
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
		private Func<ShipState, IIdResolver, AttackShipCombat> _combatFactory;
		private Weapon _weapon;
		private Mock<IRandom> _random;
		private Ship _target;
		private AttackShipCombat _combat;

		[SetUp]
		public void SetUp()
		{
			_random = new Mock<IRandom>(MockBehavior.Strict);

			Func<ShipState, ObjectBuilder<Ship>> ship = state => Ship.Builder.Build(null, state);
			_target = new Ship {Statistics = new ShipStatistics(Stats())};
			_weapon = new Weapon();
			
			_combatFactory = (state, idResolver) => new AttackShipCombat(_random.Object) { Ship = ship(state).Build(idResolver), Target = _target };
			_combat = _combatFactory(new ShipState(ItemCode.LightFrigate), new IdResolutionContext(new[] { new ShipInfo() { Code = ItemCode.LightFrigate } }));
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
		public void FireAll()
		{
			var items = new IObject[] { new ShipInfo { Code = ItemCode.LightFrigate }, new WeaponInfo { Code = ItemCode.MissileLauncher, MinimumDamage = 100, MaximumDamage = 100 }, };

			var ship = new ShipState(ItemCode.LightFrigate)
			{
				LocalCoordinates = Vector.Parse("0,-1"),
				Heading = Vector.Parse("0,-1"),
				HardPoints = new[]
				{
					new HardPointState { Weapon = new WeaponState {Code = ItemCode.MissileLauncher}, Position = HardPointPosition.Front },
					new HardPointState { Weapon = new WeaponState {Code = ItemCode.MissileLauncher}, Position = HardPointPosition.Rear },
					new HardPointState { Weapon = new WeaponState {Code = ItemCode.MissileLauncher}, Position = HardPointPosition.Top },
				}
			};

			_combat = _combatFactory(ship, new IdResolutionContext(items));
			
			Assert.That(_combat.Ship.HardPoints, Is.Not.Empty);

			_random.Setup(x => x.GetNext()).Returns(0);

			var result =
				_combat.Ship.HardPoints
				.Where(hp => hp.Weapon != null)
				.Where(hp => hp.InRange(_combat.Target))
				.Select(x => _combat.Fire(x.Weapon))
				.ToArray();

			Assert.That(result, Is.Not.Empty);

			var dmg = result.Aggregate((Damage )null, (current, value) => current + value.TotalDamage);
			Assert.That(dmg.Value, Is.Not.EqualTo(0d));
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
		public void Damage()
		{
			_weapon.WeaponInfo = new WeaponInfo { DamageType = DamageType.Energy, MinimumDamage = 50, MaximumDamage = 150 };

			var rollIndex = 0;
			var rolls = new[] { 0d, 1d };

			_random.Setup(x => x.GetNext()).Returns(() => rolls[rollIndex++]);
			var r1 = _combat.Fire(_weapon);

			Assert.That(r1.TotalDamage.DamageType, Is.EqualTo(DamageType.Energy));

			Assert.That(r1.TotalDamage.Value, Is.EqualTo(150d));
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