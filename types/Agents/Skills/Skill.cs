using Echo.Statistics;

namespace Echo.Agents.Skills
{
	public class Skill
	{
		public AgentStatistic PrimaryStat { get; set; }
		public AgentStatistic SecondaryStat { get; set; }
		public int TrainingMultiplier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public SkillCode Code { get; set; }

		public SkillLevel[] Prerequisites { get; set; }
	}
}