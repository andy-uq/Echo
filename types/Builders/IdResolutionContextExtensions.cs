using System;
using System.Collections.Generic;
using System.Linq;

namespace Echo.Builder
{
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