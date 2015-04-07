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
					ObjectId = item.Id,
					Type = item.ItemInfo.Type,
					Code = item.ItemInfo.Code,
					Quantity = item.Quantity,
				};
			}

			public static ObjectBuilder<Item> Build(ItemState state, ILocation location = null, Corporation owner = null)
			{
				var objRef = state.Type.ToObjectReference(state.Code);						
				var item = new Item
				{
					Id = state.ObjectId,
					Quantity = state.Quantity,
					Location = location,
					Owner = owner,
				};

				var builder = new ObjectBuilder<Item>(item);
				
				builder
					.Resolve((resolver, target) => item.ItemInfo = resolver.Get<ItemInfo>(objRef));

				return builder;
			}
		}
	}
}