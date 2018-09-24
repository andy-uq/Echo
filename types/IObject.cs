using System.Collections.Generic;
using Echo;
using Echo.State;

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
}