using System;
using System.Collections.Generic;
using Echo.Items;
using Echo.Market;
using Echo.State.Market;
using Echo.Structures;
using Echo;

namespace Echo.State
{
	public class StructureState : IObjectState
	{
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }

		public ObjectReference Orbits { get; set; }
		public ObjectReference Owner { get; set; }

		public IEnumerable<HangerItemState> HangerItems { get; set; }
		public IEnumerable<BuyOrderState> BuyOrders { get; set; }
		public IEnumerable<SellOrderState> SellOrders { get; set; }

		public StructureType StructureType { get; set; }
		public ManufactoryState Manufactory { get; set; }
		public TradingStationState TradingStation { get; set; }
	}
}