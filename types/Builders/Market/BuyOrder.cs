using Echo.Builders;
using Echo.Ships;
using Echo.State.Market;

namespace Echo.Market
{
	partial class BuyOrder
	{
		public class Builder
		{
			public BuyOrder Build(ILocation location, BuyOrderState state)
			{
				return new BuyOrder(location.Position.GetMarketPlace())
				{
					Id = state.Auction.Id,
					Name = state.Auction.Name,
					BlockSize = state.Auction.BlockSize,
					Expires = state.Auction.Expires,
					PricePerUnit = state.Auction.PricePerUnit,
					Range = state.Auction.Range,
					Item = Echo.Items.Item.Builder.Build(state.Auction.Item)
				};
			}
		}
	}
}