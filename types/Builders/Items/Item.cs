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
					Type = item.ItemInfo.Type,
					Code = item.ItemInfo.Code,
					Quantity = item.Quantity,
				};
			}

			public static Item Build(ItemState state, IIdResolver resolver)
			{
				var itemInfo = resolver.Get<ItemInfo>(state.Code.ToObjectReference(state.Type));
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