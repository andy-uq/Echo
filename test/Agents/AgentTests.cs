using System;
using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Corporations;
using Echo.State;
using Echo.Statistics;
using Echo.Structures;
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

			agent.Skills[SkillCode.SpaceshipCommand].Level.ShouldBe(0);
			agent.Implants[AgentStatistic.Intelligence].Value.ShouldBe(0);
		}

		[Test]
		public void CanBeHired()
		{
			var state = new AgentState();
			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());

			var corporation = new Corporation();
			corporation.Hire(agent);

			agent.Corporation.ShouldBe(corporation);
			corporation.Employees.ShouldContain(agent);
		}

		[Test]
		public void CanMove()
		{
			var state = new AgentState();
			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());

			var structure = new Manufactory();
			agent.MoveInto(structure);

		}

		[Test]
		public void CantHireAgentEmployedBySomeoneElse()
		{
			var state = new AgentState();
			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());

			var c1 = new Corporation();
			c1.Hire(agent);

			var c2 = new Corporation();
			Should.Throw<ArgumentException>(() => c2.Hire(agent));
		}

		[Test]
		public void CanBeFired()
		{
			var state = new AgentState();
			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());

			var corporation = new Corporation();
			corporation.Hire(agent);

			corporation.Fire(agent);
			agent.Corporation.ShouldBe(null);
			corporation.Employees.ShouldNotContain(agent);
		}
	}
}