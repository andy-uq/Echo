using System;

namespace Echo.Agents.Skills
{
	public class CategoryAttribute : Attribute
	{
		private readonly SkillCategory _category;

		public SkillCategory Category
		{
			get { return _category; }
		}

		public CategoryAttribute(SkillCategory category)
		{
			_category = category;
		}
	}
}