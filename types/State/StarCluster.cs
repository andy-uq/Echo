using System.Collections.Generic;

namespace Echo.State
{
	public class StarClusterState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public IEnumerable<SolarSystemState> SolarSystems { get; set; }
	}
}