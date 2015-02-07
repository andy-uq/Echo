using System.Collections.Generic;
using System.Linq;
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

		public abstract IEnumerable<IObject> Values { get; }

		protected abstract bool LookupValue<T>(ulong id, out IObject value) where T : class, IObject;

		public T GetById<T>(ulong id) where T : class, IObject
		{
			IObject value;
			if ( LookupValue<T>(id, out value) )
				return (T)value;

			throw AddLookup(new ItemNotFoundException(typeof (T).Name, id));
		}

		public bool TryGetById<T>(ulong id, out T value) where T : class, IObject
		{
			IObject rawValue;
			if ( !LookupValue<T>(id, out rawValue) )
				rawValue = null;
			
			value = rawValue as T;
			return value != null;
		}

		public T Get<T>(ObjectReference objectReference) where T : class, IObject
		{
			Ensure.That(objectReference.Id).IsNotEqualTo(0UL);

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

		public static IIdResolver Empty
		{
			get { return new IdResolutionContext(Enumerable.Empty<IObject>()); }
		}
	}

	public static class IdResolverExtensions
	{
		public static IIdResolver Combine(this IIdResolver left, IIdResolver right)
		{
			return new CompositeIdResolver(left, right);
		}
	}

	public class CompositeIdResolver : IdResolver
	{
		private readonly IIdResolver[] _resolvers;

		public CompositeIdResolver(params IIdResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public override IEnumerable<IObject> Values
		{
			get { return _resolvers.SelectMany(x => x.Values); }
		}

		protected override bool LookupValue<T>(ulong id, out IObject value)
		{
			foreach (var child in _resolvers)
			{
				if ( child.TryGetById(id, out value) )
					return true;
			}

			value = null;
			return false;
		}
	}
}