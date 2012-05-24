using System.Diagnostics;

using Echo.Entities;

namespace Echo.Objects
{
	public interface IItem : IObject
	{
		/// <summary>Amount of cargo space taken by this item</summary>
		double CargoSpace { get; }

		/// <summary>Number of items</summary>
		uint Quantity { get; }

		/// <summary>Item ID</summary>
		int ItemID { get; }

		/// <summary>True if multiple items can be stacked as one</summary>
		bool Stackable { get; }

		/// <summary>Owner of the item</summary>
		Corporation Owner { get; set; }

		/// <summary>Stack more of this item</summary>
		/// <param name="item">Item to stack</param>
		void Stack(IItem item);

		/// <summary>Unstack this item</summary>
		/// <param name="quantity">Number of items to unstack</param>
		/// <returns></returns>
		IItem Unstack(uint quantity);

		/// <summary>Destroys the item</summary>
		void Destroy();
	}
}