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
					Id = item.Id,
					Name = item.Name,
					Quantity = item.Quantity,
				};
			}

			public static Item Build(ItemState state)
			{
				return new Item
				{
					Id = state.Id,
					Name = state.Name,
					Quantity = state.Quantity,
				};
			}
		}
	}
}