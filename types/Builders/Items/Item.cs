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
					Id = item.Id,
					Code = item.ItemInfo.Code,
					Quantity = item.Quantity,
				};
			}

			public static Item Build(ItemState state, IIdResolver resolver)
			{
				var itemInfo = state.Code.GetItemInfo(resolver);
				var item = new Item
				{
					Id = state.Id,
					Quantity = state.Quantity,
					ItemInfo = itemInfo
				};

				return item;
			}
		}
	}
}