using System;
using System.Diagnostics;

using Echo.Entities;

namespace Echo.Objects
{
	[DebuggerDisplay("{Name} x {Quantity}")]
	public abstract class Item : BaseObject, IItem
	{
		protected Item()
		{
			Quantity = 1;
			SizePerUnit = 1;
		}

		/// <summary>Size per unit</summary>
		public double SizePerUnit { get; set; }

		#region IItem Members

		public override ObjectType ObjectType
		{
			get { return ObjectType.Item; }
		}

		/// <summary>Item ID</summary>
		public abstract int ItemID { get; }

		/// <summary>True if multiple items can be stacked as one</summary>
		public virtual bool Stackable
		{
			get { return true; }
		}

		/// <summary>Stack more of this item</summary>
		/// <param name="item">Item to stack</param>
		public void Stack(IItem item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item.ItemID != ItemID)
				throw new InvalidOperationException("{0} cannot be stacked with {1}.".Expand(item.Name, Name));

			if (item.Stackable == false)
				throw new InvalidOperationException("{0} is not stackable.".Expand(item.Name));

			if (item.Location != Location)
				throw new InvalidOperationException("{0} is in a different location so cannot be stacked.".Expand(item.Name));

			if (item.Owner != Owner)
				throw new InvalidOperationException("{0} is owned by a different corporation so cannot be stacked.".Expand(item.Name));

			Quantity += item.Quantity;
		}

		/// <summary>Unstack this item</summary>
		/// <param name="quantity">Number of items to unstack</param>
		/// <returns></returns>
		public IItem Unstack(uint quantity)
		{
			if (quantity > Quantity)
				throw new InvalidOperationException("Request to unstack more items than is currently in this stack");
            
			var clone = (Item) MemberwiseClone(true);

			Quantity -= quantity;
			clone.Quantity = quantity;

			return clone;
		}

		/// <summary>Destroys the item</summary>
		public void Destroy()
		{
			Owner.RemoveAsset(this);
			owner = null;
		}

		private Corporation owner;

		/// <summary>Owner of the item</summary>
		public Corporation Owner
		{
			get { return this.owner; }
			set
			{
				if (this.owner == value)
					return;

				if (this.owner != null && this.owner.Assets.Contains(this))
					throw new InvalidOperationException("The previous owner still holds this item as an asset.");

				this.owner = value;
				this.owner.AddAsset(this);
			}
		}

		/// <summary>Number of items</summary>
		public uint Quantity { get; set; }

		/// <summary>Total size of this item</summary>
		public double CargoSpace
		{
			get { return Quantity*SizePerUnit; }
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0:n0}x{1}", Quantity, Name);
		}
	}
}