using System;
using Echo.Agents;
using Echo.Builders;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Agents : StateTest
	{
		public AgentState John
		{
			get { return Universe.John; }
		}


		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(John);
			DumpObjects("Agent");
		}

		[Test]
		public void Save()
		{
			var agent = Agent.Builder.Build(null, John).Materialise();
			Assert.That(agent, Is.InstanceOf<Agent>());
			var state = agent.Save();

			Assert.That(state.Statistics, Is.Not.Null);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(John);
			var state = Database.UseOnceTo().GetById<AgentState>(John.Id);
			Assert.That(state, Is.Not.Null);

			var agent = Agent.Builder.Build(null, state).Materialise();
			Assert.That(agent, Is.InstanceOf<Agent>());

			Assert.That(agent.Statistics.Charisma.Value, Is.EqualTo(50));
			Assert.That(agent.Statistics.Memory.Value, Is.EqualTo(50));
			Assert.That(agent.Statistics.Perception.Value, Is.EqualTo(50));
			Assert.That(agent.Statistics.Intelligence.Value, Is.EqualTo(50));
			Assert.That(agent.Statistics.Willpower.Value, Is.EqualTo(50));

			Assert.That(agent.Statistics.Intelligence.CurrentValue, Is.EqualTo(65));
			Assert.That(agent.Statistics.Willpower.CurrentValue, Is.EqualTo(65));
		}
	}
}