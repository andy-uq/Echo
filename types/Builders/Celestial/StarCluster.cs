using System.Linq;
using Echo.Builders;
using Echo.State;
using Echo;

namespace Echo.Celestial
{
	partial class StarCluster
	{
		public static class Builder
		{
			public static StarClusterState Save(StarCluster starCluster)
			{
				return new StarClusterState
				{
					Id = starCluster.Id,
					Name = starCluster.Name,
					LocalCoordinates = starCluster.Position.LocalCoordinates,
					SolarSystems = starCluster.SolarSystems.Save(),
				};
			}

			public static ObjectBuilder<StarCluster> Build(Universe universe, StarClusterState state)
			{
				var starCluster = new StarCluster
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(universe, state.LocalCoordinates),
				};

				var builder = new ObjectBuilder<StarCluster>(starCluster);
				builder
					.Dependents(state.SolarSystems)
					.Build(SolarSystem.Builder.Build)
					.Resolve((target, resolver, dependent) => target.SolarSystems.Add(dependent));

				return builder;
			}
		}
	}
}