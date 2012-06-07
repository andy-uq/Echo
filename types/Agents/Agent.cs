using System.Collections.Generic;
using Echo.Agents.Skills;
using Echo.Statistics;

namespace Echo.Agents
{
	public partial class Agent : IObject
	{
		public Agent()
		{
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.Agent; }
		}

		public long Id { get; private set; }
		public string Name { get; private set; }
		public ILocation Location { get; private set; }
		public AgentStatistics Statistics { get; set; }
		public Dictionary<AgentStatistic, Implant> Implants { get; set; }
		public Dictionary<SkillCode, SkillLevel> Skills { get; set; }
	}
}