using System;

namespace Echo.Items
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class CategoryAttribute : Attribute
	{
		public ItemType Type { get; }

		public CategoryAttribute(ItemType type)
		{
			Type = type;
		}
	}
}