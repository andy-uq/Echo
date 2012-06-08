using System;

namespace Echo.State.Market
{
	public class AuctionState : IObjectState
	{
		public Guid Id { get; set; }
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public uint BlockSize { get; set; }
		public ulong Expires { get; set; }
		public uint PricePerUnit { get; set; }
		public uint Range { get; set; }

		public ObjectReference Owner { get; set; }
		public ObjectReference Trader { get; set; }
	
		public ItemState Item { get; set; }
	}
}