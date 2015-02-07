using System;
using System.Collections.Generic;
using System.Linq;
using Echo.State;

namespace Echo.Agents.Skills
{
	public static class SkillCodeExtensions
	{
		public const ulong SKILL_ID_MASK = (1 << 62);

		public static ulong ToId(this SkillCode code)
		{
			return SKILL_ID_MASK | (ulong) code;
		}

		public static ObjectReference ToObjectReference(this SkillCode code)
		{
			return new ObjectReference(code.ToId(), code.ToString());
		}

		public static IEnumerable<SkillCategory> GetSkillCategories(this SkillCode skillCode)
		{
			var type = typeof(SkillCode);
			var field = type.GetField(skillCode.ToString());
			var attributes = field.GetCustomAttributes(typeof(CategoryAttribute), false);
			return attributes.Cast<CategoryAttribute>().Select(attribute => attribute.Category);
		}

		public static bool TryParse(this ulong id, out SkillCode skillCode)
		{
			skillCode = SkillCode.Invalid;

			var isSkillCode = (id & SKILL_ID_MASK) != 0L;
			if (isSkillCode)
			{
				var value = (int )(id ^ SKILL_ID_MASK);
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