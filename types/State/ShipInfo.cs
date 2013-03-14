using Echo.Ships;

namespace Echo.State
{
	public class ShipInfo : ItemInfo
	{
		public ShipClass ShipClass { get; set; }
		public SkillLevel[] PilotRequirements { get; set; }
	}
}