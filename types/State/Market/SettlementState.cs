using System.Collections.Generic;

namespace Echo.State.Market
{
	public class SettlementState
	{
		public ItemState Item { get; set; }
		public ObjectReference Owner { get; set; }
		public long TimeToSettlement { get; set; }
		public Dictionary<ObjectReference, long> SpendByOwner { get; set; }
	}
}