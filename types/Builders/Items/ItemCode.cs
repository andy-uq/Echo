using System;
using Echo.State;

namespace Echo.Items
{
	public enum ItemCode
	{
		Invalid
	}

	public static class ItemCodeExtensions
	{
		public const long ITEM_ID_MASK = (1 << 61);

		public static long ToId(this ItemCode code)
		{
			return ITEM_ID_MASK | (long)code;
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