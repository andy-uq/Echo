using System.Collections.Generic;
using Echo.State.Market;

namespace Echo.State
{
	public class TradingStationState
	{
		public IEnumerable<ObjectReference> BuyOrders { get; set; }
		public IEnumerable<ObjectReference> SellOrders { get; set; }
	}
}