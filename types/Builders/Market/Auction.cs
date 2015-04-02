using System;
using Echo.Agents;
using Echo.Builder;
using Echo.Items;
using Echo.State.Market;
using Echo.Structures;

namespace Echo.Market
{
	partial class Auction
	{
		public static class Builder
		{
			public static AuctionState Save(Auction auction)
			{
				return new AuctionState
				{
					ObjectId = auction.Id,
					Name = auction.Name,
					BlockSize = auction.BlockSize,
					Expires = auction.Expires,
					PricePerUnit = auction.PricePerUnit,
					Range = auction.Range,
					Trader = auction.Trader.ToObjectReference(),
					Owner = auction.Owner.ToObjectReference(),
					Item = Item.Builder.Save(auction.Item)
				};
			}

			public static ObjectBuilder<T> Build<T>(Structure location, AuctionState state, Func<T> createFunc) where T : Auction
			{
				var auction = createFunc();
				auction.Id = state.ObjectId;
				auction.Name = state.Name;
				auction.BlockSize = state.BlockSize;
				auction.Expires = state.Expires;
				auction.PricePerUnit = state.PricePerUnit;
				auction.Range = state.Range;
				auction.Location = location;

				return new ObjectBuilder<T>(auction)
					.Resolve((resolver, target) => target.Item = Item.Builder.Build(state.Item, resolver))
					.Resolve((resolver, target) => target.Trader = resolver.Get<Agent>(state.Trader));
			}
		}
	}
}