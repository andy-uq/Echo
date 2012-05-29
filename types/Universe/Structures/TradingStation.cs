using System.Collections.Generic;
using Echo.Market;

namespace Echo.Structures
{
	public partial class TradingStation : Structure
	{
		public override StructureType StructureType
		{
			get { return StructureType.TradingStation; }
		}

		public List<SellOrder> SellOrders { get; set; }
		public List<BuyOrder> BuyOrders { get; set; }
	}
}