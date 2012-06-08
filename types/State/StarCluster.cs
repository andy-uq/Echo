using System;
using System.Collections.Generic;

namespace Echo.State
{
	public class StarClusterState : IObjectState
	{
		public Guid Id { get; set; }
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public IEnumerable<SolarSystemState> SolarSystems { get; set; }
	}
}