using System.Collections.Generic;
using System.Linq;
using Echo.State.Market;

namespace Echo.State
{
	public class MarketPlaceState
	{
		public MarketPlaceState()
		{
			Settlements = Enumerable.Empty<SettlementState>();
			Auctions = Enumerable.Empty<ObjectReference>();
		}

		public long AuctionLength { get; set; }
		public long SettlementDelay { get; set; }
		public IEnumerable<SettlementState> Settlements { get; set; }
		public IEnumerable<ObjectReference> Auctions { get; set; }
	}
}