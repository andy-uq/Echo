using System.Collections.Generic;
using Echo.State.Market;

namespace Echo.State
{
	public class TradingStationState
	{
		public IEnumerable<BuyOrderState> BuyOrders { get; set; }
		public IEnumerable<SellOrderState> SellOrders { get; set; }
	}
}