using System;

namespace Echo.State.Market
{
	public class SellOrderState : IObjectState
	{
		public AuctionState Auction { get; set; }

		long IObjectState.ObjectId
		{
			get { return Auction.ObjectId; }
		}

		string IObjectState.Name
		{
			get { return Auction.Name; }
		}
	}
}