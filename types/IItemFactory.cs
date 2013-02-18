using System;
using Echo.Items;
using Echo.State;

namespace Echo
{
	public interface IItemFactory
	{
		Item Build(ItemCode itemCode, uint quantity);
	}

	public class ItemFactory : IItemFactory
	{
		private readonly IIdResolver _resolver;

		public ItemFactory(IIdResolver resolver)
		{
			_resolver = resolver;
		}

		#region IItemFactory Members

		public Item Build(ItemCode itemCode, uint quantity)
		{
			var item = new ItemState {Code = itemCode, Quantity = quantity};
			return Item.Builder.Build(item, _resolver);
		}

		#endregion
	}
}