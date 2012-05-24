using Echo.Entities;
using Echo.Ships;

namespace Echo.Objects
{
	public class ShipYard : Manufactory
	{
		public ShipYard(ILocation orbiting, Corporation owner)
			: base(orbiting, owner)
		{
		}

		public override StructureType StructureType
		{
			get { return StructureType.ShipYard; }
		}

		protected override string SystematicNamePrefix
		{
			get { return "SY"; }
		}

		protected override IItem CreateItem(IItem item)
		{
            AddShip((Ship) item);
			return item;
		}
	}
}