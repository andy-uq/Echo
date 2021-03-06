﻿using System.Collections.Generic;
using System.Linq;
using Echo;
using Echo.Agents.Skills;
using Echo.State;
using Echo.Statistics;
using SkillLevel = Echo.State.SkillLevel;

namespace test.common
{
	public static class TestSkills
	{
		private static readonly Dictionary<SkillCode, SkillInfo> _skills;

		static TestSkills()
		{
			_skills = Skills.ToDictionary(k => k.Code);
		}

		public static IEnumerable<SkillInfo> Skills
		{
			get
			{
				yield return new SkillInfo
				{
					Code = SkillCode.SpaceshipCommand,
					Description = "The skill required to fly a spaceship",
					Name = "Spaceship Command",
					Prerequisites = new SkillLevel[0],
					PrimaryStat = AgentStatistic.Perception,
					SecondaryStat = AgentStatistic.Charisma,
					TrainingMultiplier = 1
				};
			}
		}

		public static SkillInfo For(SkillCode skillCode)
		{
			return _skills[skillCode];
		}

		public static IIdResolver RegisterTestSkills(this IIdResolver idResolver)
		{
			return idResolver.Combine(new IdResolutionContext(Skills));
		}
	}
}