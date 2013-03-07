using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Skills;
using Echo.Statistics;

namespace Echo.Tests.Mocks
{
	public static class TestSkills
	{
		private static Dictionary<SkillCode, Skill> _skills;

		static TestSkills()
		{
			_skills = Skills.ToDictionary(k => k.Code);
		}

		private static IEnumerable<Skill> Skills
		{
			get
			{
				yield return new Skill
				{
					Code = SkillCode.SpaceshipCommand,
					Description = "The skill required to fly a spaceship",
					Name = "Spaceship Command",
					Prerequisites = new SkillLevel[0],
					PrimaryStat = AgentStatistic.Perception,
					SecondaryStat = AgentStatistic.Willpower,
					TrainingMultiplier = 1
				};
			}
		}

		public static Skill For(SkillCode skillCode)
		{
			return _skills[skillCode];
		}
	}
}