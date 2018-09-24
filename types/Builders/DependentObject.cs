using System;
using System.Collections.Generic;

namespace Echo
{
	namespace Builder
	{
		public class DependentObject<T, TDependentState> : IBuilderContext
			where T : IObject 
			where TDependentState : class, IObjectState
		{
			private readonly TDependentState _state;
			private readonly ObjectBuilder<T> _parent;
			private IBuilderContext _target;

			public DependentObject(ObjectBuilder<T> objectBuilder, TDependentState state)
			{
				_parent = objectBuilder;
				_state = state;
			}

			public Resolver<T, TDependentState, TObject> Build<TObject>(Func<T, TDependentState, ObjectBuilder<TObject>> build)
				where TObject : IObject
			{
				if ( _state != null )
				{
					var dependentObject = build(_parent.Target, _state);
					_target = dependentObject;
					return new Resolver<T, TDependentState, TObject>(_parent, dependentObject);
				}

				return new Resolver<T, TDependentState, TObject>();
			}

			public Resolver<T, TDependentState, TObject> Build<TObject>(Func<TDependentState, ObjectBuilder<TObject>> build)
				where TObject : IObject
			{
				if ( _state != null )
				{
					var dependentObject = build(_state);
					_target = dependentObject;
					return new Resolver<T, TDependentState, TObject>(_parent, dependentObject);
				}

				return new Resolver<T, TDependentState, TObject>();
			}

			public IObject Target => _target.Target;

			public IEnumerable<IBuilderContext> DependentObjects => _target.DependentObjects;

			public IObject Build(IIdResolver resolver) => _target.Build(resolver);
		}
	}
}
