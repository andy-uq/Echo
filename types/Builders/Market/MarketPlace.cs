using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using EnsureThat;

namespace Echo.Market
{
	partial class MarketPlace
	{
		public static class Builder
		{
			public static MarketPlaceState Save(MarketPlace marketPlace)
			{
				Ensure.That(marketPlace).IsNotNull();

				return new MarketPlaceState
				{
					AuctionLength = marketPlace.AuctionLength,
					SettlementDelay = marketPlace.SettlementDelay,
					Settlements = marketPlace.Settlements.Save(),
					Auctions = marketPlace.Auctions.Select(_ => _.ToObjectReference()),
				};
			}

			public static ObjectBuilder<MarketPlace> Build(StarCluster starCluster, MarketPlaceState state)
			{
				var marketPlace = new MarketPlace
				{
					AuctionLength = state.AuctionLength,
					SettlementDelay = state.SettlementDelay,
					StarCluster = starCluster,					
				};

				var builder = new ObjectBuilder<MarketPlace>(marketPlace);
				builder.Resolve((resolver, target) => target._auctions.AddRange(state.Auctions.Select(resolver.Get<Auction>)));

				return builder;
			}
		}
	}
}