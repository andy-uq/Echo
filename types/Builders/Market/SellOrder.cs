using Echo.Builders;
using Echo.Ships;
using Echo.State.Market;

namespace Echo.Market
{
	partial class SellOrder
	{
		public class Builder
		{
			public IdResolutionContext<SellOrder> Build(ILocation location, SellOrderState state)
			{
				return new IdResolutionContext<SellOrder>
				{
					Target = new SellOrder(location.Position.GetMarketPlace())
					{
						Id = state.Auction.Id,
						Name = state.Auction.Name,
						BlockSize = state.Auction.BlockSize,
						Expires = state.Auction.Expires,
						PricePerUnit = state.Auction.PricePerUnit,
						Range = state.Auction.Range,
					},
					Resolved =
						{
							(resolver, order) => order.Item = state.Auction.Item.Build(location, resolver)
						}
				};
			}
		}
	}
}