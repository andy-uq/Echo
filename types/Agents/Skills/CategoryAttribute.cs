using System;

namespace Echo.Agents.Skills
{
	public class CategoryAttribute : Attribute
	{
		public SkillCategory Category { get; }

		public CategoryAttribute(SkillCategory category)
		{
			Category = category;
		}
	}
}