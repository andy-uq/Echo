﻿using System.Dynamic;
using System.Linq;
using Echo.Builder;
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
					ObjectId = starCluster.Id,
					Name = starCluster.Name,
					LocalCoordinates = starCluster.Position.LocalCoordinates,
					SolarSystems = starCluster.SolarSystems.Save(),
					MarketPlace = starCluster.MarketPlace.Save(),
				};
			}

			public static ObjectBuilder<StarCluster> Build(Universe universe, StarClusterState state)
			{
				var starCluster = new StarCluster
				{
					Id = state.ObjectId,
					Name = state.Name,
					Position = new Position(universe, state.LocalCoordinates),
				};

				var builder = new ObjectBuilder<StarCluster>(starCluster);
				builder.Resolve((resolver, target) => target.MarketPlace = Market.MarketPlace.Builder.Build(starCluster, state.MarketPlace).Build(resolver));

				builder
					.Dependents(state.SolarSystems)
					.Build(SolarSystem.Builder.Build)
					.Resolve((resolver, target, dependent) => target.SolarSystems.Add(dependent));

				return builder;
			}
		}
	}
}