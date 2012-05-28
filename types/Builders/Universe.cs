using System.Linq;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using Echo;

namespace Echo
{
	partial class Universe
	{
		public static class Builder
		{
			public static UniverseState Save(Universe universe)
			{
				return new UniverseState
				{
					Id = universe.Id,
					Name = universe.Name,
					StarClusters = universe.StarClusters.Save(),
				};
			}

			public static Universe Build(UniverseState state)
			{
				var universe = new Universe();

				universe.StarClusters = state.StarClusters
					.Select(x => StarCluster.Builder.Build(universe, x))
					.ToList();

				return universe;
			}
		}
	}
}