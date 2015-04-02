using System.Collections.Generic;
using Echo.State.Market;

namespace Echo.State
{
	public class MarketPlaceState
	{
		public long AuctionLength { get; set; }
		public long SettlementDelay { get; set; }
		public IEnumerable<SettlementState> Settlements { get; set; }
		public IEnumerable<ObjectReference> Auctions { get; set; }
	}
}