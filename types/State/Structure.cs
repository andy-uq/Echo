using System.Collections.Generic;
using System.Linq;
using Echo.State.Market;
using Echo.Structures;

namespace Echo.State
{
	public class StructureState : IObjectState
	{
		public StructureState()
		{
			Personnel = Enumerable.Empty<ObjectReference>();
			HangerItems = Enumerable.Empty<HangarItemState>();
			BuyOrders = Enumerable.Empty<AuctionState>();
			SellOrders = Enumerable.Empty<AuctionState>();
		}

		public ulong ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }

		public ObjectReference Orbits { get; set; }
		public ObjectReference Owner { get; set; }

		public IEnumerable<ObjectReference> Personnel { get; set; }
		public IEnumerable<HangarItemState> HangerItems { get; set; }
		public IEnumerable<AuctionState> BuyOrders { get; set; }
		public IEnumerable<AuctionState> SellOrders { get; set; }

		public StructureType StructureType { get; set; }
		public ManufactoryState Manufactory { get; set; }
		public TradingStationState TradingStation { get; set; }
	}
}