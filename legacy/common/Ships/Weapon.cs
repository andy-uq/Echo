using Echo.Objects;

namespace Echo.Ships
{
	public class Weapon : Item
	{
		private int itemID;

		public Weapon(WeaponBlueprint blueprint)
		{
			this.itemID = blueprint.BuildItemID;

			AmmoItemID = blueprint.AmmoItemID;
			Quantity = blueprint.BuildQuantity;
			MinDamage = blueprint.MinDamage;
			MaxDamage = blueprint.MaxDamage;
			Speed = blueprint.Speed;
			Range = blueprint.Range;
		}

		public int? AmmoItemID { get; private set; }

		public override int ItemID
		{
			get { return itemID; }
		}

		public uint MinDamage { get; set; }
		public uint MaxDamage { get; set; }
		public double Speed { get; set; }
		public double Range { get; set; }
	}
}