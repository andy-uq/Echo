using System.Linq;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Builder;
using Echo.State;

using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;
using AgentSkillLevel = Echo.Agents.Skills.SkillLevel;
using SkillLevel = Echo.State.SkillLevel;

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
					Implants = new ImplantCollection(state.Implants)
				};

				return new ObjectBuilder<Agent>(agent)
					.Resolve((r, a) => a.Location = r.Get<ILocation>(state.Location))
					.Resolve((r, a) => a.Skills = new SkillCollection(state.Skills.Select(skill => Build(r, skill))))
					.Resolve(ApplyStatDeltas);
			}

			public static AgentState Save(Agent agent)
			{
				return new AgentState
				{
					ObjectId = agent.Id,
					Name = agent.Name,
					Statistics = agent.Statistics.Select(Save),
					Skills = agent.Skills.Select(Save),
					Location = agent.Location.AsObjectReference()
				};
			}
			
			private static void ApplyStatDeltas(IIdResolver idResolver, Agent agent)
			{
				foreach (var implant in agent.Implants)
				{
					agent.Statistics[implant.Stat].Alter(implant);
				}

				agent.Statistics.Recalculate();
			}

			private static AgentStatisticValue Build(AgentStatisticState x)
			{
				return new AgentStatisticValue(x.Statistic, x.Value);
			}

			private static AgentSkillLevel Build(IIdResolver resolver, SkillLevel x)
			{
				return new AgentSkillLevel { Skill = resolver.Get<SkillInfo>(x.SkillCode.ToObjectReference()), Level = x.Level };
			}

			private static AgentStatisticState Save(AgentStatisticValue x)
			{
				return new AgentStatisticState {Statistic = x.Stat, CurrentValue = x.CurrentValue, Value = x.Value,};
			}

			private static SkillLevel Save(AgentSkillLevel x)
			{
				return new SkillLevel(x.Skill.Code, x.Level);
			}
		}
	}
}