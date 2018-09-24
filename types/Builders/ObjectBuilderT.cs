using System.Collections.Generic;

namespace Echo
{
	namespace Builder
	{
		public class ObjectBuilder<T> : ObjectBuilder, IBuilderContext
			where T : IObject
		{
			private readonly List<ResolveHandler<T>> _resolveActions;

			public T Target { get; }

			public ObjectBuilder(T target)
			{
				Target = target;
				_resolveActions = new List<ResolveHandler<T>>();
			}

			#region IBuilderContext Members

			IObject IBuilderContext.Target => Target;
			IObject IBuilderContext.Build(IIdResolver idResolver) => Build(idResolver);

			#endregion

			public T Build(IIdResolver idResolver)
			{
				foreach (var dependent in DependentObjects)
					dependent.Build(idResolver);

				foreach (var action in _resolveActions)
					action(idResolver, Target);

				return Target;
			}

			public ObjectBuilder<T> Resolve(ResolveHandler<T> onResolve)
			{
				_resolveActions.Add(onResolve);
				return this;
			}
		}
	}
}