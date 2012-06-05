using System.Collections.Generic;

namespace Echo.State
{
	public class ShipState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }

		public IEnumerable<HardPointState> HardPoints { get; set; }
	}
}