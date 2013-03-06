using System;

namespace Echo.Items
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class CategoryAttribute : Attribute
	{
		public ItemCategory Category { get; set; }

		public CategoryAttribute(ItemCategory category)
		{
			Category = category;
		}
	}
}