using System.Collections.Generic;
using System.Linq;
using Echo.Statistics;
using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;

namespace Echo.State
{
	public class AgentState : IObjectState
	{
		public AgentState()
		{
			Statistics = Enumerable.Empty<AgentStatisticState>();
			Implants = Enumerable.Empty<Implant>();
			Skills = Enumerable.Empty<SkillLevel>();
			Training = Enumerable.Empty<SkillTraining>();
		}

		public ulong ObjectId { get; set; }
		public string Name { get; set; }
		public ObjectReference? Location { get; set; }
		public IEnumerable<AgentStatisticState> Statistics { get; set; }
		public IEnumerable<Implant> Implants { get; set; }
		public IEnumerable<SkillLevel> Skills { get; set; }
		public IEnumerable<SkillTraining> Training { get; set; }
	}

	public class Implant
	{
		public AgentStatistic Stat { get; set; }
		public int Rank { get; set; }
		public int Value { get; set; }
	}

	public class AgentStatisticState : StatisticState<int>
	{
		public AgentStatistic Statistic { get; set; }
	}
}