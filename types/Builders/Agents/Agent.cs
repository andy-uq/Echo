using System.Linq;
using Echo.Builder;
using Echo.State;

using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;

namespace Echo.Agents
{
	partial class Agent
	{
		public static class Builder
		{
			public static ObjectBuilder<Agent> Build(AgentState state)
			{
				var agent = new Agent
				{
					Id = state.ObjectId,
					Name = state.Name,
					Statistics = new AgentStatistics(state.Statistics.Select(Build)),
					Implants = (state.Implants ?? Enumerable.Empty<Implant>()).ToDictionary(x => x.Stat)
				};

				return new ObjectBuilder<Agent>(agent)
					.Resolve((r, a) => a.Location = r.Get<ILocation>(state.Location))
					.Resolve(ApplyStatDeltas);
			}

			public static AgentState Save(Agent agent)
			{
				return new AgentState
				{
					ObjectId = agent.Id,
					Name = agent.Name,
					Statistics = agent.Statistics.Select(Save),
					Location = agent.Location.AsObjectReference()
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