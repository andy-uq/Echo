using System;

namespace Echo.Agents.Skills
{
	public class CategoryAttribute : Attribute
	{
		public SkillCategory Category { get; set; }

		public CategoryAttribute(SkillCategory category)
		{
			Category = category;
		}
	}
}