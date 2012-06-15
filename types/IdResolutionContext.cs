using System;
using System.Linq;
using System.Collections.Generic;
using Echo.Exceptions;
using Echo.State;
using EnsureThat;

namespace Echo
{
	public class IdResolutionContext : IIdResolver
	{
		private readonly Dictionary<long, IObject> _lookup;

		public IdResolutionContext(IEnumerable<IObject> collection)
		{
			collection = collection.ToArray();

			var q =
				(
					from o in collection
					group o by o.Id
					into g
					where g.Count() > 1
					select string.Format("{0}: {1}", g.Key, string.Join(", ", g.Select(x => x.Name)))
				).ToArray();

			if (q.Any())
				throw new InvalidOperationException("Duplicate keys detected: " + string.Join("; ", q));

			_lookup = collection.Where(x => x.Id != 0).ToDictionary(x => x.Id);
		}

		public T GetById<T>(long id) where T : class, IObject
		{
			Ensure.That(id, "id").IsGte(0L);
			return (T)_lookup[id];
		}

		public bool TryGetById<T>(long id, out T value) where T : class, IObject
		{
			IObject rawValue;
			if ( !_lookup.TryGetValue(id, out rawValue) )
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

			throw new ItemNotFoundException(typeof(T).Name, objectReference);
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