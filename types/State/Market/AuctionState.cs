namespace Echo.State.Market
{
	public class AuctionState : IObjectState
	{
		public ulong ObjectId { get; set; }
		public string Name { get; set; }
		public uint BlockSize { get; set; }
		public long Expires { get; set; }
		public int PricePerUnit { get; set; }
		public double Range { get; set; }

		public ObjectReference Owner { get; set; }
		public ObjectReference Trader { get; set; }
	
		public ItemState Item { get; set; }
	}
}