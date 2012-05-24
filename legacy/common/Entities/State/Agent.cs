using System;
using System.Collections.Generic;
using System.Xml;

using Echo.Objects;

namespace Echo.Entities
{
	public partial class Agent
	{
		internal class AgentState<T> : BaseObjectState<Agent> where T : Corporation, new()
		{
			public AgentState(Universe.UniverseState universeState) : base(universeState)
			{
			}

			protected override Agent ReadXml(XmlElement xAgent)
			{
				Agent agent = base.ReadXml(xAgent);
				agent.Stats = new AgentStatistics();
				agent.Employer = Universe.GetObject<T>(xAgent, "employer");

				foreach ( AgentStatistic statType in Enum.GetValues(typeof(AgentStatistic)) )
				{
					XmlElement xStat;
					if (xAgent.Select("stats/stat[@type='{0}']".Expand(statType), out xStat) == false)
						continue;

					var value = xStat.Int32("value");
					var currentValue = xStat.Int32("current");
					agent.Stats[statType].SetValue(value, currentValue);
				}

				return agent;
			}

			protected override void WriteXml(Agent obj, XmlElement xAgent)
			{
				base.WriteXml(obj, xAgent);
				xAgent.Element("employer", obj.Employer);

				var xStats = xAgent.Append("stats");
				foreach (AgentStatistic statType in Enum.GetValues(typeof(AgentStatistic)))
				{
					var value = obj.Stats[statType];
					var xStat = xStats.Append("stat");

					xStat.Attribute("type", statType);
					xStat.Attribute("value", value.Value);
					xStat.Attribute("current", value.CurrentValue);
				}
			}
		}
	}
}