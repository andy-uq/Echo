using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Implants;
using Echo.Agents.Skills;
using Echo.Corporations;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using EnsureThat;

namespace Echo.Agents
{
	public partial class Agent : IObject
	{
		public Agent()
		{
			Implants = new ImplantCollection();
			Skills = new SkillCollection();
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.Agent; }
		}

		public ulong Id { get; private set; }
		public string Name { get; private set; }
		public Corporation Corporation { get; set; }
		public ILocation Location { get; set; }
		public AgentStatistics Statistics { get; set; }
		public ImplantCollection Implants { get; set; }
		public SkillCollection Skills { get; set; }

		public bool CanUse(Ship ship)
		{
			Ensure.That(ship).IsNotNull();
			return CanUse(ship.ShipInfo);
		}

		private bool CanUse(ShipInfo shipInfo)
		{
			Ensure.That(shipInfo).IsNotNull();
			return CanUse(shipInfo.PilotRequirements);
		}

		public bool CanUse(BluePrintInfo bluePrint)
		{
			Ensure.That(bluePrint).IsNotNull();
			return CanUse(bluePrint.BuildRequirements);
		}

		private bool CanUse(IEnumerable<State.SkillLevel> requirements)
		{
			return requirements.All(requirement => Skills[requirement.SkillCode].Level >= requirement.Level);
		}

		public void MoveInto(ILocation location)
		{
			Location = location;
			var structure = location as Structure;
			if (structure != null)
			{
				structure.Personnel.Add(this);
			}
		}
	}
}