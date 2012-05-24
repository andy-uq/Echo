using System.Linq;
using Echo.Celestial;
using Echo.State;
using Echo;

namespace Echo
{
	partial class Universe
	{
		public class Builder
		{
			public Universe Build(UniverseState state)
			{
				var universe = new Universe();

				var starClusterBuilder = new StarCluster.Builder();
				universe.StarClusters = state.StarClusters
					.Select(x => starClusterBuilder.Build(universe, x))
					.ToList();

				return universe;
			}
		}
	}
}