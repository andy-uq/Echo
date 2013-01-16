using System;
using System.Collections.Generic;
using System.Linq;
using Echo.State;

namespace Echo.Items
{
	public static class ItemCodeExtensions
	{
		public const long ITEM_ID_MASK = (1 << 61);

		public static long ToId(this ItemCode code)
		{
			if (code == ItemCode.Invalid)
				throw new InvalidOperationException("Cannot generate an Id for an Invalid item");

			return ITEM_ID_MASK | (long)code;
		}

		public static ObjectReference ToObjectReference(this ItemCode code)
		{
			return new ObjectReference(ToId(code), code.ToString());
		}

		public static IEnumerable<ItemCategory> GetItemCategories(this ItemCode itemCode)
		{
			var type = typeof (ItemCode);
			var field = type.GetField(itemCode.ToString());
			var attributes = field.GetCustomAttributes(typeof (CategoryAttribute), false);
			return attributes.Cast<CategoryAttribute>().Select(attribute => attribute.Category);
		}

		public static ItemInfo GetItemInfo(this ItemCode itemCode, IIdResolver resolver)
		{
			return resolver.GetById<ItemInfo>(ToId(itemCode));
		}

		public static bool TryParse(this long id, out ItemCode itemCode)
		{
			itemCode = ItemCode.Invalid;

			var isSkillCode = (id & ITEM_ID_MASK) != 0L;
			if ( isSkillCode )
			{
				int value = (int)(id ^ ITEM_ID_MASK);
				if ( Enum.IsDefined(typeof(ItemCode), value) )
				{
					itemCode = (ItemCode)(id ^ ITEM_ID_MASK);
					return (itemCode != ItemCode.Invalid);
				}
			}

			return false;
		}
	}
}