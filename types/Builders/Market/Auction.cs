using System;
using Echo.Agents;
using Echo.Builder;
using Echo.Corporations;
using Echo.Items;
using Echo.State.Market;
using Echo.Structures;
using EnsureThat;

namespace Echo.Market
{
	partial class Auction
	{
		public static class Builder
		{
			public static AuctionState Save(Auction auction)
			{
				Ensure.That(auction.Trader).IsNotNull();
				Ensure.That(auction.Owner).IsNotNull();

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
					.Resolve((resolver, target) => target.Item = BuildItem<T>(resolver, state))
					.Resolve((resolver, target) => target.Trader = resolver.Get<Agent>(state.Trader));
			}

			private static Item BuildItem<T>(IIdResolver resolver, AuctionState state) where T : Auction
			{
				var item = Item.Builder.Build(state.Item, resolver);
				item.Owner = resolver.Get<Corporation>(state.Owner);
				return item;
			}
		}
	}
}