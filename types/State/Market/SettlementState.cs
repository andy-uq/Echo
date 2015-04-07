using System.Collections.Generic;

namespace Echo.State.Market
{
	public class SettlementState : IObjectState
	{
		public ItemState Item { get; set; }
		public ObjectReference Location { get; set; }
		public ObjectReference Owner { get; set; }
		public long TimeToSettlement { get; set; }
		public Dictionary<ObjectReference, long> SpendByOwner { get; set; }

		public ulong ObjectId { get; set; }
		public string Name { get; set; }
	}
}