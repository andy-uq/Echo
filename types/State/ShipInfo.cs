using Echo.Items;
using Echo.Ships;

namespace Echo.State
{
	public class ShipInfo : ItemInfo
	{
		public ShipClass ShipClass { get; set; }
		public SkillLevel[] PilotRequirements { get; set; }
		
		public override ItemType Type
		{
			get { return ItemType.Ships; }
			set {  }
		}

		public override ObjectType ObjectType
		{
			get { return ObjectType.Ship; }
		}
	}
}