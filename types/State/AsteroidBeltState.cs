using Echo.Items;

namespace Echo.State
{
	public class AsteroidBeltState
	{
		public ItemCode Ore { get; set; }
		public uint AmountRemaining { get; set; }
		public int Richness { get; set; }
	}
}