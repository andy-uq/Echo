using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Skills;

namespace Echo.State
{
	public class SkillLevel : IEquatable<SkillLevel>
	{
		private static readonly Lazy<SkillLevel[]> _defaultSkillLevels = new Lazy<SkillLevel[]>(() => SkillCodes.All.Select(x => new SkillLevel(x, 0)).ToArray());
		public static IEnumerable<SkillLevel> DefaultSkillLevels => _defaultSkillLevels.Value;

		public SkillLevel()
		{
		}

		public SkillLevel(SkillCode skillCode, int level)
		{
			SkillCode = skillCode;
			Level = level;
		}

		public SkillCode SkillCode { get; set; }
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
	}
}