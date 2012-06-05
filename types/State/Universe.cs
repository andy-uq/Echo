using System.Collections.Generic;
using Echo;

namespace Echo.State
{
	public class UniverseState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<StarClusterState> StarClusters { get; set; }
	}
}