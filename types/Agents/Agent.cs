using System.Collections.Generic;
using Echo.Agents.Skills;
using Echo.Ships;
using Echo.State;
using Echo.Statistics;
using EnsureThat;
using SkillLevel = Echo.Agents.Skills.SkillLevel;

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
		public ILocation Location { get; set; }
		public AgentStatistics Statistics { get; set; }
		public Dictionary<AgentStatistic, Implant> Implants { get; set; }
		public Dictionary<SkillCode, SkillLevel> Skills { get; set; }

		public bool CanUse(Ship ship)
		{
			Ensure.That(() => ship).IsNotNull();
			return CanUse(ship.ShipInfo);
		}

		private bool CanUse(ShipInfo shipInfo)
		{
			Ensure.That(() => shipInfo).IsNotNull();
			return CanUse(shipInfo.PilotRequirements);
		}

		public bool CanUse(BluePrintInfo bluePrint)
		{
			Ensure.That(() => bluePrint).IsNotNull();
			return CanUse(bluePrint.BuildRequirements);
		}

		private bool CanUse(IEnumerable<State.SkillLevel> requirements)
		{
			foreach ( var requirement in requirements )
			{
				SkillLevel skill;
				if ( !Skills.TryGetValue(requirement.SkillCode, out skill) )
					return false;

				if ( skill.Level < requirement.Level )
					return false;
			}

			return true;
		}
	}
}