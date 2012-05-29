using Echo.Corporations;

namespace Echo.State
{
	public class ItemState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }

		public long Owner { get; set; }
		public uint Quantity { get; set; }
	}
}