using System;
using System.Collections.Generic;
using System.Linq;

namespace Echo.Agents.Skills
{
	public static class SkillCodeExtensions
	{
		public const long SKILL_ID_MASK = (1 << 62);

		public static long ToId(this SkillCode code)
		{
			return SKILL_ID_MASK | (long) code;
		}

		public static IEnumerable<SkillCategory> GetSkillCategories(this SkillCode skillCode)
		{
			var type = typeof(SkillCode);
			var field = type.GetField(skillCode.ToString());
			var attributes = field.GetCustomAttributes(typeof(CategoryAttribute), false);
			return attributes.Cast<CategoryAttribute>().Select(attribute => attribute.Category);
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