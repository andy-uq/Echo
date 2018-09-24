using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Exceptions;
using Echo.State;

namespace Echo
{
	public abstract class IdResolver : IIdResolver
	{
		protected ItemNotFoundException AddLookup(ItemNotFoundException itemNotFoundException)
		{
			foreach ( var item in Values )
				itemNotFoundException.Data[item.Id] = item.AsObjectReference();

			return itemNotFoundException;
		}

		public abstract IEnumerable<IObject> Values { get; }

		protected abstract bool LookupValue<T>(ulong id, out IObject value) where T : class, IObject;

		public T GetById<T>(ulong id) where T : class, IObject
		{
			if (LookupValue<T>(id, out var value))
				return (T)value;

			throw AddLookup(new ItemNotFoundException(typeof (T).Name, id));
		}

		public bool TryGetById<T>(ulong id, out T value) where T : class, IObject
		{
			if ( !LookupValue<T>(id, out var rawValue) )
				rawValue = null;
			
			value = rawValue as T;
			return value != null;
		}

		public T Get<T>(ObjectReference objectReference) where T : class, IObject
		{
			if (objectReference.Id <=0)
				throw new InvalidOperationException($"Invalid id: {objectReference.Id}");

			if ( TryGetById(objectReference.Id, out T value) )
				return value;

			throw AddLookup(new ItemNotFoundException(typeof(T).Name, objectReference));
		}

		public bool TryGet<T>(ObjectReference objectReference, out T value) where T : class, IObject
		{
			return TryGetById(objectReference.Id, out value);
		}

		public T Get<T>(ObjectReference? objectReference) where T : class, IObject
		{
			if ( objectReference == null )
				return null;

			return Get<T>(objectReference.Value);
		}

		public bool TryGet<T>(ObjectReference? objectReference, out T value) where T : class, IObject
		{
			if ( objectReference == null )
			{
				value = null;
				return false;
			}
			
			return TryGet(objectReference.Value, out value);
		}

		public static IIdResolver Empty => new IdResolutionContext(Enumerable.Empty<IObject>());
	}

	public static class IdResolverExtensions
	{
		public static IIdResolver Combine(this IIdResolver left, IIdResolver right)
		{
			return new CompositeIdResolver(left, right);
		}
	}
}