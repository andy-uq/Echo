using System;

namespace Echo.State.Market
{
	public class BuyOrderState : IObjectState
	{
		public AuctionState Auction { get; set; }

		public Guid Id
		{
			get { return Auction.Id; }
		}

		public long ObjectId
		{
			get { return Auction.ObjectId; }
		}

		public string Name
		{
			get { return Auction.Name; }
		}
	}
}