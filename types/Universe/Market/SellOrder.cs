namespace Echo.Market
{
	public partial class SellOrder : Auction
	{
		public override Settlement List(MarketPlace marketPlace)
		{
			return new Settlement();
		}
	}
}