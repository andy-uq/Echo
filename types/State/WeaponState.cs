using Echo.Items;

namespace Echo.State
{
	public class WeaponState : IObjectState
	{
		public ulong ObjectId { get; set; }
		public string Name { get; set; }

		public ItemCode Code { get; set; }
	}
}