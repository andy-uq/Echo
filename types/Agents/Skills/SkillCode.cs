using System;

namespace Echo.Agents.Skills
{
	public enum SkillCode
	{
		Invalid,

		[Category(SkillCategory.SpaceshipCommand)]
		SpaceshipCommand,
	}

	public static class SkillCodeExtensions
	{
		public const long SKILL_ID_MASK = (1 << 62);

		public static long ToId(this SkillCode code)
		{
			return SKILL_ID_MASK | (long) code;
		}

		public static bool TryParse(this long id, out SkillCode skillCode)
		{
			skillCode = SkillCode.Invalid;

			var isSkillCode = (id & SKILL_ID_MASK) != 0L;
			if (isSkillCode)
			{
				int value = (int )(id ^ SKILL_ID_MASK);
				if (Enum.IsDefined(typeof (SkillCode), value))
				{
					skillCode = (SkillCode) (id ^ SKILL_ID_MASK);
					return (skillCode != SkillCode.Invalid);
				}
			}

			return false;
		}
	}
}