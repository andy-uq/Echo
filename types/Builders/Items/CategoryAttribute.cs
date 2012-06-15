using System;

namespace Echo.Items
{
	public class CategoryAttribute : Attribute
	{
		public ItemCategory Category { get; set; }

		public CategoryAttribute(ItemCategory category)
		{
			Category = category;
		}
	}
}