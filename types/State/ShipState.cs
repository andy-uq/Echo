﻿using System.Collections.Generic;
using Echo.Statistics;

namespace Echo.State
{
	public class ShipState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
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