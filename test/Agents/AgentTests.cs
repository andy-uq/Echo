using System;
using Echo.Agents;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Corporations;
using Echo.State;
using Echo.Statistics;
using Echo.Structures;
using NUnit.Framework;
using Shouldly;
using test.common;
using SkillLevel = Echo.State.SkillLevel;

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
		public void CreateAgentWithImplants()
		{
			var state = new AgentState
			{
				Statistics = new[] { new AgentStatisticState { Statistic = AgentStatistic.Intelligence, Value = 500 } },
				Implants = new[] { new State.Implant { Stat = AgentStatistic.Intelligence, Rank = 6, Value = 1000 } }
			};

			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());

			agent.Statistics.Intelligence.CurrentValue.ShouldBe(1500);

			agent.Implants[AgentStatistic.Intelligence].Rank.ShouldBe(6);
			agent.Implants[AgentStatistic.Intelligence].Value.ShouldBe(1000);
		}

		[Test]
		public void CreateAgentWithSkills()
		{
			var state = new AgentState
			{
				Skills = new[] { new SkillLevel(SkillCode.SpaceshipCommand, 5) }
			};

			var agent = Agent.Builder.Build(state).Build(new TestIdResolver());
			agent.Skills[SkillCode.SpaceshipCommand].Level.ShouldBe(5);
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
			Should.Throw<InvalidOperationException>(() => c2.Hire(agent));
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