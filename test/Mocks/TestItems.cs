﻿using System.Collections.Generic;
using System.Linq;
using Echo.Builder;
using Echo.Items;
using Echo.State;

namespace Echo.Tests.Mocks
{
	public static class TestItems
	{
		private static readonly Dictionary<ItemCode, ItemInfo> _Items;

		static TestItems()
		{
			_Items = Items.ToDictionary(k => k.Code);
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
					Name = "Missile Launcher",
				};

				yield return new ItemInfo
				{
					Code = ItemCode.Veldnium,
					Name = "Veldnium",
				};
			}
		}

		public static ItemInfo For(ItemCode itemCode)
		{
			return _Items[itemCode];
		}

		public static IIdResolver RegisterTestItems(this IIdResolver resolver)
		{
			return resolver.Combine(new IdResolutionContext(Items));
		}
	}
}