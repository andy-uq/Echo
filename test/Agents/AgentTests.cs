using Echo.Agents;
using Echo.Agents.Skills;
using Echo.State;
using Echo.Statistics;
using NUnit.Framework;
using Shouldly;
using test.common;

namespace Echo.Tests.Agents
{
	[TestFixture]
	public class AgentTests
	{
		[Test]
		public void CreateAgent()
		{
			var state = new AgentState();

			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());
			agent.ObjectType.ShouldBe(ObjectType.Agent);
			agent.Corporation.ShouldBe(null);
			agent.Location.ShouldBe(null);

			agent.Statistics.Intelligence.CurrentValue.ShouldBe(0);

			agent.Skills.ShouldContainKey(SkillCode.SpaceshipCommand);
			agent.Skills[SkillCode.SpaceshipCommand].Level.ShouldBe(0);

			agent.Implants.ShouldContainKey(AgentStatistic.Intelligence);
			agent.Implants[AgentStatistic.Intelligence].Value.ShouldBe(0);
		}

		[Test]
		public void CanBeHired()
		{
			var state = new AgentState();

			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());
			agent.ObjectType.ShouldBe(ObjectType.Agent);
			agent.Corporation.ShouldBe(null);
			agent.Location.ShouldBe(null);

			agent.Statistics.Intelligence.CurrentValue.ShouldBe(0);

			agent.Skills.ShouldContainKey(SkillCode.SpaceshipCommand);
			agent.Skills[SkillCode.SpaceshipCommand].Level.ShouldBe(0);

			agent.Implants.ShouldContainKey(AgentStatistic.Intelligence);
			agent.Implants[AgentStatistic.Intelligence].Value.ShouldBe(0);
		}
	}
}