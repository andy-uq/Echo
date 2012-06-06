using System.Linq;
using Echo.Builder;
using Echo.State;

using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;

namespace Echo.Agents
{
	partial class Agent : IObject
	{
		 public static class Builder
		 {
		 	public static ObjectBuilder<Agent> Build(ILocation location, AgentState state)
		 	{
		 		var agent = new Agent
		 		{
		 			Id = state.Id,
					Name = state.Name,
					Location = location,
					Statistics = new AgentStatistics(state.Statistics.Select(Build)),
					Implants = (state.Implants ?? Enumerable.Empty<Implant>()).ToDictionary(x => x.Stat)
		 		};

				return new ObjectBuilder<Agent>(agent)
					.Resolve(ApplyStatDeltas);
		 	}

		 	public static AgentState Save(Agent agent)
		 	{
		 		return new AgentState
		 		{
		 			Id = agent.Id,
		 			Name = agent.Name,
		 			Statistics = agent.Statistics.Select(Save)
		 		};
		 	}

			private static void ApplyStatDeltas(IIdResolver idResolver, Agent agent)
			{
				foreach (var implant in agent.Implants.Values)
				{
					agent.Statistics[implant.Stat].Alter(implant);
				}

				agent.Statistics.Recalculate();
			}

		 	private static AgentStatisticValue Build(AgentStatisticState x)
			{
				return new AgentStatisticValue(x.Statistic, x.Value);
			}

		 	private static AgentStatisticState Save(AgentStatisticValue x)
		 	{
		 		return new AgentStatisticState {Statistic = x.Stat, CurrentValue = x.CurrentValue, Value = x.Value,};
		 	}
		 }
	}
}