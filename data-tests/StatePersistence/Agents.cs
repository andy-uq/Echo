using System;
using Echo.Agents;
using Echo.Builders;
using Echo.Data.Tests;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Agents : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
			public AgentState Value { get; set; }

			public WrappedObjectState(AgentState value)
			{
				Value = value;
			}
		}

		public AgentState John
		{
			get { return Universe.John; }
		}


		[Test]
		public void Persist()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(John));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var agent = Agent.Builder.Build(John).Materialise();
			Assert.That(agent, Is.InstanceOf<Agent>());
			var state = agent.Save();

			Assert.That(state.Statistics, Is.Not.Null);

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(John);
			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);

				var agent = Agent.Builder.Build(state).Materialise();
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
}