using System.Collections.Generic;

namespace Echo.State
{
	public class StarClusterState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public List<SolarSystemState> SolarSystems { get; set; }
	}
}