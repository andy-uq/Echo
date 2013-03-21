using System;

namespace Echo.State.Market
{
	public sealed class BuyOrderState : IObjectState
	{
		public AuctionState Auction { get; set; }

		long IObjectState.ObjectId
		{
			get { return Auction == null ? 0 : Auction.ObjectId; }
		}

		string IObjectState.Name
		{
			get { return Auction == null ? null : Auction.Name; }
		}
	}
}