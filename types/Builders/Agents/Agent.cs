﻿using System.Linq;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Builder;
using Echo.State;

using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;
using AgentSkillLevel = Echo.Agents.Skills.SkillLevel;
using AgentSkillTraining = Echo.Agents.Skills.SkillTraining;

using SkillLevel = Echo.State.SkillLevel;
using SkillTraining = Echo.State.SkillTraining;

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

			private static AgentSkillLevel Build(IIdResolver resolver, SkillLevel x)
			{
				var skill = resolver.Get<SkillInfo>(x.SkillCode.ToObjectReference());
				return new AgentSkillLevel(skill, x.Level);
			}

			private static AgentSkillTraining Build(IIdResolver resolver, SkillTraining x)
			{
				var skill = resolver.Get<SkillInfo>(x.SkillCode.ToObjectReference());
				return new AgentSkillTraining(skill) { Remaining = x.Remaining, Start = x.Start, Complete = x.Complete, Paused = x.Paused };
			}

			private static AgentStatisticState Save(AgentStatisticValue x)
			{
				return new AgentStatisticState {Statistic = x.Stat, CurrentValue = x.CurrentValue, Value = x.Value};
			}

			private static SkillLevel Save(AgentSkillLevel x)
			{
				return new SkillLevel(x.Skill.Code, x.Level);
			}

			private static SkillTraining Save(AgentSkillTraining x)
			{
				return new SkillTraining { SkillCode = x.SkillCode, Remaining = x.Remaining, Start = x.Start, Complete = x.Complete, Paused = x.Paused };
			}
		}
	}
}