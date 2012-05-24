using System.Collections.Generic;

using Echo.Ships;

namespace Echo.Objects
{
	public abstract class Blueprint : Item
	{
		public override int ItemID
		{
			get { return -2; }
		}

		/// <summary>True if multiple items can be stacked as one</summary>
		public override bool Stackable
		{
			get { return false; }
		}

		public uint BuildCost { get; set; }
		public uint BuildQuantity { get; set; }
		public int BuildItemID { get; set; }

		public List<Material> Materials { get; set; }

		public abstract IItem CreateItem();
	}

	public class Material
	{
		public int ItemID { get; private set; }
		public string Name { get; private set; }
		public uint Quantity { get; private set; }

		public Material(int itemID, string name, uint quantity)
		{
			ItemID = itemID;
			Name = name;
			Quantity = quantity;
		}
	}

	public class WeaponBlueprint : Blueprint
	{
		public int? AmmoItemID { get; set; }

		public uint MinDamage { get; set; }

		public uint MaxDamage { get; set; }

		public double Speed { get; set; }

		public double Range { get; set; }

		public override IItem CreateItem()
		{
			var item = new Weapon(this);

			return item;
		}
	}

	public class ShipBlueprint : Blueprint
	{
		public ShipBlueprint()
		{
			Stats = new ShipStatistics();
		}

		public HardPoint[] HardPoints { get; set; }

		public double Speed { get; set; }

		public ShipStatistics Stats { get; set; }

		public override IItem CreateItem()
		{
			return new Ship(this);
		}
	}
}