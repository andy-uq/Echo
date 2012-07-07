using Echo.Items;

namespace Echo.State
{
	public class WeaponState : IObjectState
	{
		public long ObjectId { get; set; }
		public string Name { get; set; }

		public ItemCode Code { get; set; }
	}
}