using System.Collections.Generic;
using Echo;
using Echo.State;
using EnsureThat;

namespace Echo
{
	public interface IObject
	{
		ObjectType ObjectType { get; }
		ulong Id { get; }
		string Name { get; }
	}

	public class ObjectEqualityComparer
		: IEqualityComparer<IObject>
	{
		public static readonly ObjectEqualityComparer Comparer = new ObjectEqualityComparer();

		bool IEqualityComparer<IObject>.Equals(IObject x, IObject y)
		{
			return Equals(x, y);
		}

		int IEqualityComparer<IObject>.GetHashCode(IObject obj)
		{
			return GetHashCode(obj);
		}

		public static bool Equals(IObject x, IObject y)
		{
			if ( x == null && y == null )
				return true;

			if ( x == null || y == null )
				return false;

			return (x.Id == y.Id);
		}

		public static int GetHashCode(IObject obj)
		{
			if ( obj == null )
				return 0;

			return obj.Id.GetHashCode();
		}
	}

	public static class ObjectExtensions
	{
		public static ObjectReference? AsObjectReference(this IObject @object)
		{
			if ( @object == null )
				return null;

			return new ObjectReference(@object.Id, @object.Name);
		}

		public static ObjectReference? AsObjectReference(this IObjectState state)
		{
			if ( state == null )
				return null;

			return new ObjectReference(state.ObjectId, state.Name);
		}

		public static ObjectReference ToObjectReference(this IObject @object)
		{
			Ensure.That(@object, "object").IsNotNull();
			return new ObjectReference(@object.Id, @object.Name);
		}

		public static ObjectReference ToObjectReference(this IObjectState state)
		{
			Ensure.That(state, "state").IsNotNull();
			return new ObjectReference(state.ObjectId, state.Name);
		}
	}
}