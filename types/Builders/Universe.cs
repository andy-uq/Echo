using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.Corporations;
using Echo.State;

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
					ObjectId = universe.Id,
					Name = universe.Name,
					StarClusters = universe.StarClusters.Save(),
					Corporations = universe.Corporations.Save(),
				};
			}

			public static ObjectBuilder<Universe> Build(UniverseState state)
			{
				var universe = new Universe();

				var builder = new ObjectBuilder<Universe>(universe);
				
				builder
					.Dependents(state.Corporations)
					.Build(Corporation.Builder.Build)
					.Resolve((resolver, target, dependent) => target.Corporations.Add(dependent));
				
				builder
					.Dependents(state.Items.Concat(state.Weapons))
					.Build(BuildInfo)
					.Resolve((resolver, target, itemInfo) => target.Items.Add(itemInfo.Code, itemInfo));
				
				builder
					.Dependents(state.Ships)
					.Build(BuildInfo)
					.Resolve((resolver, target, shipInfo) => target.Ships.Add(shipInfo.Code, shipInfo));
				
				builder
					.Dependents(state.Skills)
					.Build(BuildInfo)
					.Resolve((resolver, target, skillInfo) => target.Skills.Add(skillInfo.Code, skillInfo));
				
				builder
					.Dependents(state.StarClusters)
					.Build(StarCluster.Builder.Build)
					.Resolve((resolver, target, dependent) => target.StarClusters.Add(dependent));

				return builder;
			}

			private static ObjectBuilder<T> BuildInfo<T>(T value) where T : IObject, IObjectState
			{
				return new ObjectBuilder<T>(value);
			}
		}
	}
}