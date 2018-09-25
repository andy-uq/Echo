using Echo.Items;

namespace Echo.State
{
	public class ItemState : IObjectState
	{
		public ulong ObjectId { get; set; }
		public string Name { get; set; }

		public ItemCode Code { get; set; }
		public ItemType Type { get; set; }
		public uint Quantity { get; set; }
	}
}