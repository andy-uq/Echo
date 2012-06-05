using System;
using System.Collections.Generic;
using System.Linq;
using Echo.JumpGates;

namespace Echo
{
	public interface IIdResolver
	{
		T GetById<T>(long id) where T : class, IObject;
		bool TryGetById<T>(long id, out T value) where T : class, IObject;
	}
	
	public abstract class ObjectBuilder : IResolutionContext
	{
		private readonly IObject _target;
		private readonly HashSet<IResolutionContext> _dependents = new HashSet<IResolutionContext>(new ResolutionContextComparer());

		private class ResolutionContextComparer : IEqualityComparer<IResolutionContext>
		{
			public bool Equals(IResolutionContext x, IResolutionContext y)
			{
				return x.Target.Id.Equals(y.Target.Id);
			}

			public int GetHashCode(IResolutionContext obj)
			{
				return obj.Target.Id.GetHashCode();
			}
		}

		protected ObjectBuilder(IObject target)
		{
			_target = target;
		}

		public IEnumerable<IResolutionContext> Dependents
		{
			get { return _dependents; }
		}

		public void Add(IResolutionContext dependent)
		{
			_dependents.Remove(dependent);
			_dependents.Add(dependent);
		}

		IObject IResolutionContext.Resolve(IIdResolver idResolver)
		{

			return _target;
		}

		IObject IResolutionContext.Target
		{
			get { return _target; }
		}
	}

	public class ObjectBuilder<T> : ObjectBuilder
		where T : IObject
	{
		public ObjectBuilder(T target)
			: base(target)
		{
			Target = target;
			Connect = new List<Action<IIdResolver, T>>();
		}

		public T Target { get; private set; }

		public List<Action<IIdResolver, T>> Connect { get; set; }

		public T Resolve(IIdResolver idResolver)
		{
			foreach ( var dependent in Dependents )
				dependent.Resolve(idResolver);

			foreach ( var action in Connect )
				action(idResolver, Target);

			return Target;
		}
	}

	public class Dependent<T, TDependentState> : IResolutionContext
		where T : IObject where TDependentState : IObjectState
	{
		public Dependent(ObjectBuilder<T> objectBuilder, TDependentState state)
		{
			Object = objectBuilder;
			State = state;
		}

		private TDependentState State { get; set; }
		public ObjectBuilder<T> Object { get; set; }
		public IResolutionContext DependentObject { get; set; }

		public Resolver<T, TDependentState, TObject> Build<TObject>(Func<T, TDependentState, ObjectBuilder<TObject>> build) 
			where TObject : IObject
		{
			var dependentObject = build(Object.Target, State);
			DependentObject = dependentObject;
			return new Resolver<T, TDependentState, TObject>(Object, dependentObject);
		}

		public Resolver<T, TDependentState, TObject> Build<TObject>(Func<TDependentState, ObjectBuilder<TObject>> build) 
			where TObject : IObject
		{
			var dependentObject = build(State);
			DependentObject = dependentObject;
			return new Resolver<T, TDependentState, TObject>(Object, dependentObject);
		}

		IObject IResolutionContext.Target
		{
			get { return DependentObject.Target; }
		}

		public IEnumerable<IResolutionContext> Dependents
		{
			get { return DependentObject.Dependents; }
		}

		public IObject Resolve(IIdResolver resolver)
		{
			return DependentObject.Resolve(resolver);
		}
	}
	
	public class Resolver<T, TDependentState, TDependent> : IResolutionContext
		where T : IObject 
		where TDependentState : IObjectState
		where TDependent : IObject
	{
		private readonly ObjectBuilder<T> _target;
		private readonly ObjectBuilder<TDependent> _dependent;
		private Func<IIdResolver, TDependent> _resolveObject;

		public Resolver(ObjectBuilder<T> target, ObjectBuilder<TDependent> dependent)
		{
			_target = target;
			_dependent = dependent;
			_resolveObject = dependent.Resolve;

			_target.Add(this);
		}

		
		public Resolver<T, TDependentState, TDependent> Resolve(ResolveHandler<T, TDependent> resolve)
		{
			_resolveObject = resolver => resolve(_target.Target, resolver, _resolveObject(resolver));
			return this;
		}

		public Resolver<T, TDependentState, TDependent> Resolve(FinalResolveHandler<T, TDependent> resolve)
		{
			var prev = _resolveObject;
			_resolveObject = resolver =>
			                 	{
			                 		var dependent = prev(resolver);
			                 		resolve(_target.Target, resolver, dependent);
			                 		return dependent;
			                 	};
			return this;
		}

		IObject IResolutionContext.Target
		{
			get { return _dependent.Target; }
		}

		public IEnumerable<IResolutionContext> Dependents
		{
			get { return _dependent.Dependents; }
		}

		IObject IResolutionContext.Resolve(IIdResolver idResolver)
		{
			return _resolveObject(idResolver);
		}
	}

	public delegate TDependent ResolveHandler<in T, TDependent>(T target, IIdResolver idResolver, TDependent dependent)
			where T : IObject
			where TDependent : IObject
	;

	public delegate void FinalResolveHandler<in T, in TDependent>(T target, IIdResolver idResolver, TDependent dependent)
			where T : IObject
			where TDependent : IObject
	;

	public static class IdResolutionContextExtensions
	{
		public static Dependent<T, TDependentState> Dependent<T, TDependentState>(this ObjectBuilder<T> context, TDependentState childState)
			where T : IObject
			where TDependentState : IObjectState
		{
			return new Dependent<T, TDependentState>(context, childState);
		}

		public static IEnumerable<Dependent<T, TDependentState>> Dependents<T, TDependentState>(this ObjectBuilder<T> context, IEnumerable<TDependentState> childStates)
			where T : IObject where TDependentState : IObjectState
		{
			return childStates == null 
				? Enumerable.Empty<Dependent<T, TDependentState>>() 
				: childStates.Select(x => new Dependent<T, TDependentState>(context, x)).ToArray();
		}

		public static IEnumerable<Resolver<T, TDependentState, TObject>> Build<T, TDependentState, TObject>(this IEnumerable<Dependent<T, TDependentState>> dependents, Func<T, TDependentState, ObjectBuilder<TObject>> build)
			where T : IObject
			where TDependentState : IObjectState
			where TObject : IObject
		{
			return dependents == null
			       	? Enumerable.Empty<Resolver<T, TDependentState, TObject>>()
			       	: dependents
			       	  	.Select(d => d.Build(build))
			       	  	.ToArray();
		}

		public static IEnumerable<Resolver<T, TDependentState, TObject>> Build<T, TDependentState, TObject>(this IEnumerable<Dependent<T, TDependentState>> dependents, Func<TDependentState, ObjectBuilder<TObject>> build)
			where T : IObject
			where TDependentState : IObjectState 
			where TObject : IObject
		{
			return dependents
				.Select(d => d.Build(build))
				.ToArray();
		}

		public static IEnumerable<Resolver<T, TDependentState, TObject>> Resolve<T, TDependentState, TObject>(this IEnumerable<Resolver<T, TDependentState, TObject>> resolvers, ResolveHandler<T, TObject> resolve)
			where T : IObject
			where TDependentState : IObjectState 
			where TObject : IObject
		{
			return resolvers
				.Select(r => r.Resolve(resolve))
				.ToArray();
		}

		public static IEnumerable<Resolver<T, TDependentState, TObject>> Resolve<T, TDependentState, TObject>(this IEnumerable<Resolver<T, TDependentState, TObject>> resolvers, FinalResolveHandler<T, TObject> resolve)
			where T : IObject
			where TDependentState : IObjectState 
			where TObject : IObject
		{
			return resolvers
				.Select(r => r.Resolve(resolve))
				.ToArray();
		}

		public static IEnumerable<IObject> FlattenObjectTree(this IResolutionContext context)
		{
			yield return context.Target;
			foreach (var child in context.Dependents.SelectMany(x => x.FlattenObjectTree()))
			{
				yield return child;
			}
		}
	}

	public interface IResolutionContext
	{
		IObject Target { get; }
		IEnumerable<IResolutionContext> Dependents { get; }
		IObject Resolve(IIdResolver idResolver);
	}
}