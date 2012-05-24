using System;
using System.Xml;

using Echo.Maths;
using Echo.Objects;
using Echo.Objects.Templates;

namespace Echo.Entities
{
	public partial class Agent
	{
		#region Nested type: AgentTemplate

		public class Template : BaseTemplate<Agent>
		{
			public Template(ObjectFactory universe) : base(universe)
			{
				DefaultAgentStats = new AgentStatistics();
				StatVariation = 0.5d;
			}

			public AgentStatistics DefaultAgentStats { get; set; }
			public double StatVariation { get; set; }

			public Agent Create(Corporation employer)
			{
				var agentStats = new AgentStatistics(DefaultAgentStats);
				foreach ( AgentStatistic statType in Enum.GetValues(typeof(AgentStatistic)) )
				{
					double variation = 1d + (Rand.Next() * StatVariation * 2d) - StatVariation;
					var stat = agentStats[statType];

					stat.SetValue((int )Math.Round(stat.Value * variation, 0));
				}
                
				return new Agent(employer) {  Location = Universe, Stats = new AgentStatistics(agentStats), Name = string.Format("Drone {0}", employer.Employees.Count) };
			}

			protected override void ReadXml(XmlElement xAgent)
			{
				base.ReadXml(xAgent);

				var xStats = xAgent.Select("stats");
				StatVariation = xStats.Double("variation");
				
				foreach ( AgentStatistic statType in Enum.GetValues(typeof(AgentStatistic)) )
				{
					XmlElement xStat;
					if ( xStats.Select("stat[@type='{0}']".Expand(statType), out xStat) == false )
						continue;

					var value = xStat.Int32("value");
					var currentValue = xStat.Int32("current");

					DefaultAgentStats[statType].SetValue(value, currentValue);
				}
			}
            
			protected override void WriteXml(XmlElement xAgent)
			{
				base.WriteXml(xAgent);

				var xStats = xAgent.Append("stats");
				xStats.Attribute("variation", StatVariation);

				foreach ( AgentStatistic statType in Enum.GetValues(typeof(AgentStatistic)) )
				{
					var value = DefaultAgentStats[statType];
					var xStat = xStats.Append("stat");

					xStat.Attribute("type", value.Stat);
					xStat.Attribute("value", value.Value);
					xStat.Attribute("current", value.CurrentValue);
				}
			}
		}

		#endregion
	}
}