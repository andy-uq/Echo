using System.Collections.Generic;
using System.Linq;
using Echo.Items;
using Echo.Statistics;

namespace Echo.State
{
	public class ShipState : IObjectState
	{
		public string Id { get; set; }
		public ulong ObjectId { get; set; }
		public string Name { get; set; }
		public ItemCode Code { get; set; }
		
		public Vector LocalCoordinates { get; set; }
		public Vector Heading { get; set; }
		
		public AgentState Pilot { get; set; }

		public IEnumerable<ShipStatisticState> Statistics { get; set; }
		public IEnumerable<HardPointState> HardPoints { get; set; }

		public ShipState()
		{
			Statistics = Enumerable.Empty<ShipStatisticState>();
			HardPoints = Enumerable.Empty<HardPointState>();
		}

		public ShipState(ItemCode code) : this()
		{
			Code = code;
		}
	}

	public class ShipStatisticState : StatisticState<double>
	{
		public ShipStatistic Statistic { get; set; }
	}
}