using System;
using System.Collections.Generic;
using System.Linq;
using Echo.State;

namespace Echo.Items
{
	public static class ItemCodeExtensions
	{
		public const ulong GENERIC_ITEM_ID_MASK = (1 << 61);
		public const ulong WEAPON_ID_MASK = (1 << 60);
		public const ulong BLUEPRINT_ID_MASK = (1 << 59);
		public const ulong SHIP_ID_MASK = (1 << 58);
		public const ulong ITEM_ID_MASK = GENERIC_ITEM_ID_MASK | WEAPON_ID_MASK | BLUEPRINT_ID_MASK | SHIP_ID_MASK;

		private static Dictionary<ItemType, ulong> _masks = new Dictionary<ItemType, ulong>()
		{
			{ ItemType.Unknown, GENERIC_ITEM_ID_MASK },
			{ ItemType.ShipWeapons, WEAPON_ID_MASK },
			{ ItemType.Blueprints, BLUEPRINT_ID_MASK },
			{ ItemType.Ships, SHIP_ID_MASK },
		};

		public static ulong ToId(this ItemType type, ItemCode code)
		{
			if (code == ItemCode.Invalid)
				throw new InvalidOperationException("Cannot generate an Id for an Invalid item");

			ulong mask;
			if (!_masks.TryGetValue(type, out mask))
				mask = GENERIC_ITEM_ID_MASK;

		    var code1 = (ulong)code;
		    return mask | code1;
		}

		public static ObjectReference ToObjectReference(this ItemType type, ItemCode code)
		{
			return new ObjectReference(ToId(type, code), code.ToString());
		}

		public static IEnumerable<ItemType> GetItemCategories(this ItemCode itemCode)
		{
			var type = typeof (ItemCode);
			var field = type.GetField(itemCode.ToString());
			var attributes = field.GetCustomAttributes(typeof (CategoryAttribute), false);
			return attributes.Cast<CategoryAttribute>().Select(attribute => attribute.Type);
		}

		public static bool TryParse(this ulong id, out ItemCode itemCode)
		{
			itemCode = ItemCode.Invalid;

			var isItemCode = (id & ITEM_ID_MASK) != 0L;
			if ( isItemCode )
			{
				var value = (int) (id & ~ITEM_ID_MASK);
				if (Enum.IsDefined(typeof (ItemCode), value))
				{
					itemCode = (ItemCode) value;
					return (itemCode != ItemCode.Invalid);
				}
			}

			return false;
		}
	}
}