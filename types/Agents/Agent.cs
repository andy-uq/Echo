using System.Collections.Generic;
using Echo.Agents.Skills;
using Echo.Ships;
using Echo.State;
using Echo.Statistics;

namespace Echo.Agents
{
	public partial class Agent : IObject
	{
		public Agent()
		{
			Implants = new Dictionary<AgentStatistic, Implant>();
			Skills = new Dictionary<SkillCode, SkillLevel>();
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

		public bool CanUse(Ship ship)
		{
			return false;
		}
	}
}