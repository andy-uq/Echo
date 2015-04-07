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

			public static ObjectBuilder<T> Build<T>(AuctionState state, Structure location) where T : Auction, new()
			{
				var auction = new T
				{
					Id = state.ObjectId,
					Name = state.Name,
					BlockSize = state.BlockSize,
					Expires = state.Expires,
					PricePerUnit = state.PricePerUnit,
					Range = state.Range,
					Location = location
				};

				var builder = new ObjectBuilder<T>(auction);

				builder
					.Dependent(state.Item)
					.Build(item => Item.Builder.Build(item, location))
					.Resolve((resolver, target, item) => item.Owner = resolver.Get<Corporation>(state.Owner))
					.Resolve((resolver, target, item) => target.Item = item)
					;

				builder
					.Resolve((resolver, target) => target.Trader = resolver.Get<Agent>(state.Trader));

				return builder;
			}
		}
	}
}