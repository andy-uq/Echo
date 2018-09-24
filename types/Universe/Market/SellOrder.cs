namespace Echo.Market
{
	public class SellOrder : Auction
	{
		public override Settlement List(MarketPlace marketPlace)
		{
			return new Settlement();
		}
	}
}