using System.Collections.Generic;
using Echo;

namespace Echo.State
{
	public class UniverseState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public List<StarClusterState> StarClusters { get; set; }
	}
}