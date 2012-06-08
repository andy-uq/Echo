using Echo.Agents;
using Echo.Builder;
using Echo.Items;
using Echo.State;
using Echo.State.Market;
using Echo.Structures;

namespace Echo.Market
{
	partial class BuyOrder
	{
		public static class Builder
		{
			public static ObjectBuilder<BuyOrder> Build(Structure location, BuyOrderState state)
			{
				var buyOrder = new BuyOrder
				{
					Id = state.Auction.ObjectId,
					Name = state.Auction.Name,
					BlockSize = state.Auction.BlockSize,
					Expires = state.Auction.Expires,
					PricePerUnit = state.Auction.PricePerUnit,
					Range = state.Auction.Range,
					Location = location,
				};

				return new ObjectBuilder<BuyOrder>(buyOrder)
					.Resolve((resolver, target) => target.Item = Item.Builder.Build(state.Auction.Item, resolver))
					.Resolve((resolver, target) => target.Trader = resolver.Get<Agent>(state.Auction.Trader));
			}

			public static BuyOrderState Save(BuyOrder auction)
			{
				return new BuyOrderState
				{
					Auction = new AuctionState
					{
						ObjectId = auction.Id,
						Name = auction.Name,
						BlockSize = auction.BlockSize,
						Expires = auction.Expires,
						PricePerUnit = auction.PricePerUnit,
						Range = auction.Range,
						Trader = auction.Trader.AsObjectReference(),
						Owner = auction.Owner.AsObjectReference(),
						Item = Item.Builder.Save(auction.Item)
					}
				};
			}
		}
	}
}