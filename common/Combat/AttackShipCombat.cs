using Echo.Ships;
using Echo.Statistics;

namespace Echo.Combat
{
	public class AttackShipCombat
	{
		private readonly IRandom _random;

		public Ship Ship { get; set; }
		public Ship Target { get; set; }

		public AttackShipCombat(IRandom random)
		{
			_random = random;
		}

		public double ChanceToHit(Weapon weapon)
		{
			return 1.0;
		}

		public WeaponAttackResult Fire(Weapon weapon)
		{
			if (!DidHit(weapon))
			{
				return Miss(weapon);
			}

			var damage = CalculateDamage(weapon);
			var armour = Target.Statistics.ArmourStrength(weapon.WeaponInfo.DamageType);

			if (damage.Value <= armour.CurrentValue)
			{
				armour.Alter(damage);
				return Hit(damage);
			}

			var hullDamage = new Damage(weapon.WeaponInfo.DamageType) { Value = (damage.Value - armour.CurrentValue) };
			var armourDamage = new Damage(weapon.WeaponInfo.DamageType) { Value = (armour.CurrentValue) };

			armour.Alter(armourDamage);
			Target.Statistics[ShipStatistic.HullIntegrity].Alter(hullDamage);

			return Hit(armourDamage, hullDamage);
		}

		public Damage CalculateDamage(Weapon weapon)
		{
			var roll = Roll();
			var range = weapon.WeaponInfo.MaximumDamage - weapon.WeaponInfo.MinimumDamage;
			var value = weapon.WeaponInfo.MinimumDamage + (range * roll);

			return new Damage(weapon.WeaponInfo.DamageType) { Value = value };
		}

		private bool DidHit(Weapon weapon)
		{
			var roll = Roll();
			return (roll < ChanceToHit(weapon));
		}

		protected WeaponAttackResult Miss(Weapon weapon)
		{
			return new WeaponAttackResult
			{
				Ship = Ship,
				Target = Target,

				Hit = false
			};
		}

		protected WeaponAttackResult Hit(Damage armourDamage, Damage hullDamage = null)
		{
			return new WeaponAttackResult
			{
				Ship = Ship,
				Target = Target,

				Hit = true,
				ArmourDamage = armourDamage,
				HullDamage = hullDamage
			};
		}

		private double Roll()
		{
			return _random.GetNext();
		}
	}
}