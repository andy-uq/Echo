using System;

namespace Echo.State.Market
{
	public sealed class SellOrderState : IObjectState
	{
		public AuctionState Auction { get; set; }

		ulong IObjectState.ObjectId
		{
			get { return Auction.ObjectId; }
		}

		string IObjectState.Name
		{
			get { return Auction.Name; }
		}
	}
}