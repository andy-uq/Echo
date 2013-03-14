using System;

namespace Echo.Items
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class CategoryAttribute : Attribute
	{
		public ItemType Type { get; set; }

		public CategoryAttribute(ItemType type)
		{
			Type = type;
		}
	}
}