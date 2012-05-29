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
					LocalCoordinates = item.Position.LocalCoordinates,
					Quantity = item.Quantity,
					Owner = item.Owner.Id,
				};
			}

			public static IdResolutionContext<Item> Build(ILocation location, ItemState state)
			{
				return new IdResolutionContext<Item>
				{
					Target = Build(location, state, null),
					Resolved =
						{
							(resolver, target) => target.Owner = resolver.GetById<Corporation>(state.Owner)
						}
				};
			}

			public static Item Build(ILocation location, ItemState state, IIdResolver resolver)
			{
				return new Item
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates),
					Quantity = state.Quantity,
					Owner = (resolver == null) ? null : resolver.GetById<Corporation>(state.Owner)
				};
			}
		}
	}
}