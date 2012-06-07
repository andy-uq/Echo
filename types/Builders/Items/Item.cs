using Echo.Builder;
using Echo.Corporations;
using Echo.State;

namespace Echo.Items
{
	partial class Item
	{
		public static class Builder
		{
			public static ItemState Save(Item item)
			{
				return new ItemState
				{
					Code = item.ItemInfo.Code,
					Quantity = item.Quantity,
				};
			}

			public static Item Build(ItemState state, IIdResolver resolver)
			{
				var item = new Item
				{
					Quantity = state.Quantity,
					ItemInfo = state.Code.GetItemInfo(resolver)
				};

				return item;
			}
		}
	}
}