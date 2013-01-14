using System;
using System.Collections.Generic;
using Echo.Items;
using Echo.Statistics;

namespace Echo.State
{
	public class ShipState : IObjectState
	{
		public string Id { get; set; }
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public ItemCode Code { get; set; }
		public Vector LocalCoordinates { get; set; }
		
		public AgentState Pilot { get; set; }

		public IEnumerable<ShipStatisticState> Statistics { get; set; }
		public IEnumerable<HardPointState> HardPoints { get; set; }
	}

	public class ShipStatisticState : StatisticState<double>
	{
		public ShipStatistic Statistic { get; set; }
	}
}