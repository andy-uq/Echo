using System;
using System.Collections.Generic;
using System.Linq;
using Echo.State;

namespace Echo.Agents.Skills
{
	public class SkillLevel : IEquatable<SkillLevel>
	{
		public SkillLevel(SkillInfo skill, int level)
		{
			Skill = skill;
			Level = level;
		}

		public SkillCode SkillCode => Skill.Code;
		public SkillInfo Skill { get; }
		public int Level { get; set; }

		public bool Equals(SkillLevel other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return SkillCode == other.SkillCode;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((SkillLevel) obj);
		}

		public override int GetHashCode()
		{
			return (int) SkillCode;
		}

		public static readonly SkillLevel Invalid = new SkillLevel(new SkillInfo{Code = SkillCode.Invalid}, 0);
	}
}