using System.Collections.Generic;
using Echo.Exceptions;
using Echo.State;
using EnsureThat;

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

		protected abstract IEnumerable<IObject> Values { get; }

		protected abstract bool LookupValue<T>(long id, out IObject value) where T : class, IObject;

		public T GetById<T>(long id) where T : class, IObject
		{
			Ensure.That(id, "id").IsGte(0L);
			IObject value;
			if ( LookupValue<T>(id, out value) )
				return (T)value;

			throw AddLookup(new ItemNotFoundException(typeof (T).Name, id));
		}

		public bool TryGetById<T>(long id, out T value) where T : class, IObject
		{
			IObject rawValue;
			if ( !LookupValue<T>(id, out rawValue) )
				rawValue = null;
			
			value = rawValue as T;
			return value != null;
		}

		public T Get<T>(ObjectReference objectReference) where T : class, IObject
		{
			Ensure.That(objectReference.Id, "objectReference.Id").IsGt(0);

			T value;
			if ( TryGetById(objectReference.Id, out value) )
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
	}
}