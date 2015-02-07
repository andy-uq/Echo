﻿using System.Collections.Generic;
using System.Linq;
using Echo;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.State;
using SkillLevel = Echo.State.SkillLevel;

namespace test.common
{
	public static class TestItems
	{
		private static readonly Dictionary<ItemCode, ItemInfo> _Items;
		private static readonly Dictionary<ItemCode, WeaponInfo> _Weapons;
		private static readonly Dictionary<ItemCode, BluePrintInfo> _BluePrints;

		static TestItems()
		{
			_Items = Items.ToDictionary(k => k.Code);
			_Weapons = Weapons.ToDictionary(k => k.Code);
			_BluePrints = BluePrints.ToDictionary(k => k.Code);
		}

		public static IEnumerable<ItemInfo> Items
		{
			get
			{
				yield return new ItemInfo
				{
					Code = ItemCode.EnergyShield,
					Name = "Energy Shield",
				};

				yield return new ItemInfo
				{
					Code = ItemCode.LightFrigate,
					Name = "Light Frigate",
				};

				yield return new ItemInfo
				{
					Code = ItemCode.MiningLaser,
					Name = "Mining Laser",
				};

				yield return new ItemInfo
				{
					Code = ItemCode.MissileLauncher,
					Name = "Mining Laser",
				};

				yield return new ItemInfo
				{
					Code = ItemCode.Veldnium,
					Name = "Veldnium",
				};
			}
		}

		public static IEnumerable<WeaponInfo> Weapons
		{
			get
			{
				yield return new WeaponInfo
				{
					Code = ItemCode.MissileLauncher,
					Name = "Missile Launcher",
					DamageType = DamageType.Ballistic,
					MaximumDamage = 100,
					MinimumDamage = 50,
					Speed = 1d
				};

				yield return new WeaponInfo
				{
					Code = ItemCode.MiningLaser,
					Name = "Mining Laster",
					DamageType = DamageType.Thermal,
					MaximumDamage = 1,
					MinimumDamage = 5,
					Speed = 0.75d
				};
			}
		}

		public static IEnumerable<BluePrintInfo> BluePrints
		{
			get
			{
				yield return new BluePrintInfo(ItemCode.MissileLauncher)
				{
					BuildRequirements = new[] { new SkillLevel { SkillCode = SkillCode.SpaceshipCommand, Level = 5 }, },
					Materials = new[] { new ItemState { Code = ItemCode.Veldnium, Quantity = 10 }, },
					TargetQuantity = 2,
				};

				yield return new BluePrintInfo(ItemCode.LightFrigate)
				{
					BuildRequirements = new[] { new SkillLevel { SkillCode = SkillCode.SpaceshipCommand, Level = 5 }, },
					Materials = new[]
					{
						new ItemState { Code = ItemCode.Veldnium, Quantity = 1000 },
						new ItemState { Code = ItemCode.MissileLauncher, Quantity = 10 },
						new ItemState { Code = ItemCode.MiningLaser, Quantity = 2 },
						new ItemState { Code = ItemCode.EnergyShield, Quantity = 4 },
					},
					TargetQuantity = 1,
					BuildLength = 5,
				};
			}
		}

		public static ItemState ToItemState(this ItemCode itemCode, uint quantity=1)
		{
			return new ItemState {Code = itemCode, Quantity = quantity};
		}

		public static ItemInfo Item(ItemCode itemCode)
		{
			return _Items[itemCode];
		}

		public static WeaponInfo Weapon(ItemCode itemCode)
		{
			return _Weapons[itemCode];
		}

		public static BluePrintInfo BluePrint(ItemCode itemCode)
		{
			return _BluePrints[itemCode];
		}

		public static IIdResolver RegisterTestItems(this IIdResolver resolver)
		{
			return resolver.Combine(new IdResolutionContext(Items));
		}

		public static Item BuildItem(ItemCode itemCode, uint quantity = 1)
		{
			return new Item(Item(itemCode), quantity);
		}
	}
}