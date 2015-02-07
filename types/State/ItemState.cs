using Echo.Items;

namespace Echo.State
{
	public class ItemState
	{
		public ulong Id { get; set; }
		public ItemCode Code { get; set; }
		public ItemType Type { get; set; }
		public uint Quantity { get; set; }
	}
}