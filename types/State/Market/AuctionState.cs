namespace Echo.State.Market
{
	public class AuctionState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public uint BlockSize { get; set; }
		public ulong Expires { get; set; }
		public uint PricePerUnit { get; set; }
		public uint Range { get; set; }
		public ItemState Item { get; set; }
	}
}