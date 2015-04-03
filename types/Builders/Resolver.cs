using System;
using System.Collections.Generic;
using System.Linq;

namespace Echo
{
	namespace Builder
	{
		public class Resolver<T, TDependentState, TDependent> : IBuilderContext
			where T : IObject
			where TDependentState : IObjectState
			where TDependent : IObject
		{
			private readonly ObjectBuilder<T> _target;
			private readonly ObjectBuilder<TDependent> _dependent;
			private Func<IIdResolver, TDependent> _resolveObject;

			public Resolver()
			{
				_resolveObject = resolver => default(TDependent);
			}

			public Resolver(ObjectBuilder<T> target, ObjectBuilder<TDependent> dependent)
			{
				_target = target;
				_dependent = dependent;
				_resolveObject = dependent.Build;

				_target.Add(this);
			}

			public Resolver<T, TDependentState, TDependent> Resolve(ChainedResolveHandler<T, TDependent> resolve)
			{
				var resolveObject = _resolveObject;
				_resolveObject = resolver => resolve(resolver, _target.Target, resolveObject(resolver));

				return this;
			}

			public Resolver<T, TDependentState, TDependent> Resolve(ResolveHandler<T, TDependent> resolve)
			{
				var resolveObject = _resolveObject;
				_resolveObject = resolver =>
				{
					var dependent = resolveObject(resolver);
					resolve(resolver, _target.Target, dependent);
					return dependent;
				};
				return this;
			}

			public IObject Target
			{
				get
				{
					if (_dependent == null)
						return null;

					return _dependent.Target;
				}
			}

			public IEnumerable<IBuilderContext> DependentObjects
			{
				get
				{
					if (_dependent == null)
						return Enumerable.Empty<IBuilderContext>();

					return _dependent.DependentObjects;
				}
			}

			public IObject Build(IIdResolver idResolver)
			{
				if (_dependent == null)
					return null;

				return _resolveObject(idResolver);
			}
		}
	}
}