using System;
using System.Collections.Generic;

namespace Echo.Agents.Skills
{
	public class SkillLevel
	{
		public Skill Skill { get; set; }
		public int Level { get; set; }
	}
	
	public static class SkillLevelExtensions
	{
		public static void Add(this IDictionary<SkillCode, SkillLevel> skillList, SkillLevel newSkillLevel)
		{
			SkillLevel previousSkillLevel;
			if ( skillList.TryGetValue(newSkillLevel.Skill.Code, out previousSkillLevel) )
			{
				previousSkillLevel.Level = Math.Max(newSkillLevel.Level, previousSkillLevel.Level);
				return;
			}

			skillList.Add(newSkillLevel.Skill.Code, newSkillLevel);
		}
	}
}