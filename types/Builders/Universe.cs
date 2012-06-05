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

			public static ObjectBuilder<Universe> Build(UniverseState state)
			{
				var universe = new Universe
				{
					Id = state.Id,
				};

				var builder = new ObjectBuilder<Universe>(universe);
				builder
					.Dependents(state.StarClusters)
					.Build(StarCluster.Builder.Build)
					.Resolve((target, resolver, dependent) => target.StarClusters.Add(dependent));

				return builder;
			}
		}
	}
}