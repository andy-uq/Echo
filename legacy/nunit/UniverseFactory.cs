using System;
using System.Xml;

using Echo.Entities;
using Echo.Objects;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class UniverseFactoryTests
	{
		[Test]
		public void XmlTemplate()
		{
			var universe = new Universe();
			var factory = universe.ObjectFactory;

			foreach (AgentStatistic agentStat in Enum.GetValues(typeof (AgentStatistic)))
				factory.Agent.DefaultAgentStats[agentStat].SetValue(10);

			var factoryState = factory.Save();

			factory.Load(factoryState);
		}

		[Test]
		public void Agent()
		{
			var templateXml = @"<universe nextTemplateID=""0"">
  <playercorporation templateID=""0"" startupEmployeeCount=""5"" startupCapital=""10000"" />
  <corporation templateID=""1"" startupEmployeeCount=""5"" startupCapital=""10000"" />
  <agent templateID=""2"">
    <stats variation=""0.5000"">
      <stat type=""Charisma"" value=""10"" current=""0"" />
      <stat type=""Intelligence"" value=""10"" current=""0"" />
      <stat type=""Perception"" value=""10"" current=""0"" />
      <stat type=""Memory"" value=""10"" current=""0"" />
      <stat type=""Willpower"" value=""10"" current=""0"" />
    </stats>
  </agent>
</universe>";

			var universe = new Universe();

			var xdoc = new XmlDocument();
			xdoc.LoadXml(templateXml);

			var factory = universe.ObjectFactory;

			factory.Load(xdoc.SelectRootElement());
			foreach (AgentStatistic agentStat in Enum.GetValues(typeof(AgentStatistic)))
			{
				Assert.AreEqual(10, factory.Agent.DefaultAgentStats[agentStat].Value);
			}

			var corporation = new Corporation {Location = universe};

			var agent = factory.Agent.Create(corporation);
			foreach ( AgentStatistic agentStat in Enum.GetValues(typeof(AgentStatistic)) )
			{
				Assert.GreaterOrEqual(agent.Stats[agentStat].Value, 5);
				Assert.LessOrEqual(agent.Stats[agentStat].Value, 15);
			}
		}
	}
}