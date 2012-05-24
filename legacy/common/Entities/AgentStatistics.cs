using System;
using System.Collections.Generic;

using AgentStatisticValue = Echo.StatisticValue<Echo.AgentStatistic, int>;

namespace Echo.Entities
{
	public class AgentStatistics
	{
		private readonly Dictionary<AgentStatistic, AgentStatisticValue> stats;

		public AgentStatistics() : this(null)
		{
		}

		public AgentStatistics(AgentStatistics factoryStats)
		{
			this.stats = new Dictionary<AgentStatistic, AgentStatisticValue>();

			foreach ( AgentStatistic stat in Enum.GetValues(typeof(AgentStatistic)) )
			{
				var val = (factoryStats == null) ? new AgentStatisticValue(stat, 0) : factoryStats[stat].Clone();
				this.stats.Add(stat, val);
			}
		}

		public AgentStatisticValue this[AgentStatistic stat]
		{
			get { return this.stats[stat]; }
		}
	}
}