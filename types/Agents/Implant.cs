using Echo.Statistics;

namespace Echo.Agents
{
	public class Implant : IStatisticDelta<int>
	{
		public AgentStatistic Stat { get; set; }
		public int Rank { get; set; }
		public int Value { get; set; }
	}
}