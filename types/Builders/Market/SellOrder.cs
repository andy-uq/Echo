using Echo.Builder;
using Echo.Builders;
using Echo.Ships;
using Echo.State.Market;
using Echo.Structures;

namespace Echo.Market
{
	partial class SellOrder
	{
		public static class Builder
		{
			public static ObjectBuilder<SellOrder> Build(Structure location, SellOrderState state)
			{
				var sellOrder = new SellOrder
				{
					Id = state.Auction.Id,
					Name = state.Auction.Name,
					BlockSize = state.Auction.BlockSize,
					Expires = state.Auction.Expires,
					PricePerUnit = state.Auction.PricePerUnit,
					Range = state.Auction.Range,
				};

				return new ObjectBuilder<SellOrder>(sellOrder)
					.Resolve((resolver, target) => target.Item = Items.Item.Builder.Build(state.Auction.Item, resolver));
			}

			public static SellOrderState Save(SellOrder auction)
			{
				return new SellOrderState
				{
					Auction = new AuctionState
					{
						Id = auction.Id,
						Name = auction.Name,
						BlockSize = auction.BlockSize,
						Expires = auction.Expires,
						PricePerUnit = auction.PricePerUnit,
						Range = auction.Range,
						Item = Items.Item.Builder.Save(auction.Item)
					}
				};
			}
		}
	}
}