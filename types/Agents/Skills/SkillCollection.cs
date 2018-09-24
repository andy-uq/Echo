using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.State;

namespace Echo.Agents.Skills
{
	public class SkillCollection : IEnumerable<SkillLevel>
	{
		private readonly Dictionary<SkillCode, SkillLevel> _skills;

		public SkillCollection()
		{
			_skills = new Dictionary<SkillCode, SkillLevel>();
		}

		public SkillCollection(IEnumerable<SkillLevel> skills)
		{
			_skills = skills.ToDictionary(s => s.Skill.Code);
		}

		public SkillLevel this[SkillCode skillCode]
		{
			get
			{
				if (_skills.TryGetValue(skillCode, out var skillLevel))
				{
					return skillLevel;
				}

				return new SkillLevel {Level = 0, Skill = new SkillInfo {Code = SkillCode.Invalid}};
			}
		}

		public IEnumerator<SkillLevel> GetEnumerator() => _skills.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(SkillCode skillCode, SkillLevel skillLevel)
		{
			if (_skills.ContainsKey(skillCode))
				throw new ArgumentException("Agent already possesses this skill", nameof(skillCode));

			_skills.Add(skillCode, skillLevel);
		}
	}
}