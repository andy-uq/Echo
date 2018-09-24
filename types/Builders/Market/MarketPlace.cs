using System;
using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;

namespace Echo.Market
{
	partial class MarketPlace
	{
		public static class Builder
		{
			public static MarketPlaceState Save(MarketPlace marketPlace)
			{
				if (marketPlace == null) throw new ArgumentNullException(nameof(marketPlace));

				return new MarketPlaceState
				{
					AuctionLength = marketPlace.AuctionLength,
					SettlementDelay = marketPlace.SettlementDelay,
					Settlements = marketPlace.Settlements.Save(),
					Auctions = marketPlace.Auctions.Select(_ => _.ToObjectReference()),
				};
			}

			public static ObjectBuilder<MarketPlace> Build(MarketPlaceState state, StarCluster starCluster)
			{
				var marketPlace = new MarketPlace
				{
					AuctionLength = state.AuctionLength,
					SettlementDelay = state.SettlementDelay,
					StarCluster = starCluster,					
				};

				var builder = new ObjectBuilder<MarketPlace>(marketPlace);

				builder
					.Dependents(state.Settlements)
					.Build(Settlement.Builder.Build)
					.Resolve((resolver, target, settlement) => target._settlements.Add(settlement));
				
				builder
					.Resolve((resolver, target) => target._auctions.AddRange(state.Auctions.Select(resolver.Get<Auction>)));

				return builder;
			}
		}
	}
}