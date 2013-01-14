using System;
using System.Collections.Generic;
using System.Linq;
using Echo.JumpGates;

namespace Echo
{
	namespace Builder
	{
		public abstract class ObjectBuilder
		{
			private readonly IObject _target;
			private readonly HashSet<IBuilderContext> _dependents = new HashSet<IBuilderContext>(new ResolutionContextComparer());

			private class ResolutionContextComparer : IEqualityComparer<IBuilderContext>
			{
				public bool Equals(IBuilderContext x, IBuilderContext y)
				{
					return x.Target.Id.Equals(y.Target.Id);
				}

				public int GetHashCode(IBuilderContext obj)
				{
					return obj.Target.Id.GetHashCode();
				}
			}

			protected ObjectBuilder(IObject target)
			{
				_target = target;
			}

			public IEnumerable<IBuilderContext> DependentObjects
			{
				get { return _dependents; }
			}

			public void Add(IBuilderContext dependent)
			{
				_dependents.Remove(dependent);
				_dependents.Add(dependent);
			}
		}

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

		public static class IdResolutionContextExtensions
		{
			public static DependentObject<T, TDependentState> Dependent<T, TDependentState>(this ObjectBuilder<T> context, TDependentState childState)
				where T : IObject
				where TDependentState : class, IObjectState
			{
				return new DependentObject<T, TDependentState>(context, childState);
			}

			public static ObjectBuilder<T> Dependent<T, TDependentState>(this ObjectBuilder<T> context, TDependentState childState, Action<DependentObject<T, TDependentState>> func)
				where T : IObject
				where TDependentState : class, IObjectState
			{
				var dependent = new DependentObject<T, TDependentState>(context, childState);
				func(dependent);
				
				return context;
			}

			public static IEnumerable<DependentObject<T, TDependentState>> Dependents<T, TDependentState>(this ObjectBuilder<T> context, IEnumerable<TDependentState> childStates)
				where T : IObject 
				where TDependentState : class, IObjectState
			{
				return childStates == null
				       	? Enumerable.Empty<DependentObject<T, TDependentState>>()
				       	: childStates.Select(x => new DependentObject<T, TDependentState>(context, x)).ToArray();
			}

			public static IEnumerable<Resolver<T, TDependentState, TObject>> Build<T, TDependentState, TObject>(this IEnumerable<DependentObject<T, TDependentState>> dependents, Func<T, TDependentState, ObjectBuilder<TObject>> build)
				where T : IObject
				where TDependentState : class, IObjectState
				where TObject : IObject
			{
				return dependents == null
				       	? Enumerable.Empty<Resolver<T, TDependentState, TObject>>()
				       	: dependents
				       	  	.Select(d => d.Build(build))
				       	  	.ToArray();
			}

			public static IEnumerable<Resolver<T, TDependentState, TObject>> Build<T, TDependentState, TObject>(this IEnumerable<DependentObject<T, TDependentState>> dependents, Func<TDependentState, ObjectBuilder<TObject>> build)
				where T : IObject
				where TDependentState : class, IObjectState
				where TObject : IObject
			{
				return dependents
					.Select(d => d.Build(build))
					.ToArray();
			}

			public static IEnumerable<Resolver<T, TDependentState, TObject>> Resolve<T, TDependentState, TObject>(this IEnumerable<Resolver<T, TDependentState, TObject>> resolvers, ChainedResolveHandler<T, TObject> dependentObjectResolve)
				where T : IObject
				where TDependentState : class, IObjectState
				where TObject : IObject
			{
				return resolvers
					.Select(r => r.Resolve(dependentObjectResolve))
					.ToArray();
			}

			public static IEnumerable<Resolver<T, TDependentState, TObject>> Resolve<T, TDependentState, TObject>(this IEnumerable<Resolver<T, TDependentState, TObject>> resolvers, ResolveHandler<T, TObject> resolve)
				where T : IObject
				where TDependentState : class, IObjectState
				where TObject : IObject
			{
				return resolvers
					.Select(r => r.Resolve(resolve))
					.ToArray();
			}

			public static IEnumerable<IObject> FlattenObjectTree(this IBuilderContext context)
			{
				yield return context.Target;
				foreach (var child in context.DependentObjects.SelectMany(x => x.FlattenObjectTree()))
				{
					yield return child;
				}
			}
		}
	}
}