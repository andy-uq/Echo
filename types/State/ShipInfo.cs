using Echo.Items;
using Echo.Ships;

namespace Echo.State
{
	public class ShipInfo : IObject, IObjectState
	{
		public ItemCode Code { get; set; }
		public ShipClass ShipClass { get; set; }
		public SkillLevel[] PilotRequirements { get; set; }
		public ItemCode BluePrint { get; set; }
		public string Name { get; set; }

		public ObjectType ObjectType
		{
			get { return ObjectType.Info; }
		}

		public long Id
		{
			get { return Code.ToId(); }
			set {  }
		}
	}
}