using System.Linq;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Builder;
using Echo.State;

using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;
using AgentSkillLevel = Echo.Agents.Skills.SkillLevel;
using AgentSkillTraining = Echo.Agents.Skills.SkillTraining;
using AgentImplant = Echo.Agents.Implants.Implant;

using SkillLevel = Echo.State.SkillLevel;
using SkillTraining = Echo.State.SkillTraining;
using Implant = Echo.State.Implant;

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
					Implants = new ImplantCollection(state.Implants.Select(Build))
				};

				return new ObjectBuilder<Agent>(agent)
					.Resolve((resolver, target) => target.Location = resolver.Get<ILocation>(state.Location))
					.Resolve((resolver, target) => target.Skills = new SkillCollection(state.Skills.Select(skill => Build(resolver, skill))))
					.Resolve((resolver, target) => target.Training = new SkillTrainingCollection(state.Training.Select(skill => Build(resolver, skill))))
					.Resolve(ApplyStatDeltas);
			}

			public static AgentState Save(Agent agent)
			{
				return new AgentState
				{
					ObjectId = agent.Id,
					Name = agent.Name,
					Statistics = agent.Statistics.Select(Save),
					Implants = agent.Implants.Select(Save),
					Skills = agent.Skills.Select(Save),
					Location = agent.Location.AsObjectReference(),
					Training = agent.Training.Select(Save)
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

			private static AgentImplant Build(Implant x)
			{
				return new AgentImplant(x.Stat) { Rank = x.Rank, Value = x.Value };
			}

			private static AgentSkillLevel Build(IIdResolver resolver, SkillLevel x)
			{
				var skill = resolver.Get<SkillInfo>(x.SkillCode.ToObjectReference());
				return new AgentSkillLevel(skill, x.Level);
			}

			private static AgentSkillTraining Build(IIdResolver resolver, SkillTraining x)
			{
				var skill = resolver.Get<SkillInfo>(x.SkillCode.ToObjectReference());
				return new AgentSkillTraining(skill) { Remaining = (uint) x.Remaining, Paused = x.Paused };
			}

			private static AgentStatisticState Save(AgentStatisticValue x)
			{
				return new AgentStatisticState {Statistic = x.Stat, CurrentValue = x.CurrentValue, Value = x.Value};
			}

			private static Implant Save(AgentImplant x)
			{
				return new Implant {Stat = x.Stat, Rank = x.Rank, Value = x.Rank};
			}

			private static SkillLevel Save(AgentSkillLevel x)
			{
				return new SkillLevel(x.Skill.Code, x.Level);
			}

			private static SkillTraining Save(AgentSkillTraining x)
			{
				return new SkillTraining { SkillCode = x.SkillCode, Remaining = (int)x.Remaining, Paused = x.Paused };
			}
		}
	}
}