namespace Echo.State.Market
{
	public class BuyOrderState : IObjectState
	{
		public AuctionState Auction { get; set; }

		public long Id
		{
			get { return Auction.Id; }
		}

		public string Name
		{
			get { return Auction.Name; }
		}
	}
}