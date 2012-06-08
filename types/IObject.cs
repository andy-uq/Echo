using Echo;
using Echo.State;

namespace Echo
{
	public interface IObject
	{
		ObjectType ObjectType { get; }
		long Id { get; }
		string Name { get; }
	}

	public static class ObjectExtensions
	{
		public static ObjectReference AsObjectReference(this IObject @object)
		{
			if ( @object == null )
				return new ObjectReference();

			return new ObjectReference(@object.Id, @object.Name);
		}

		public static ObjectReference AsObjectReference(this IObjectState state)
		{
			if ( state == null )
				return new ObjectReference();

			return new ObjectReference(state.ObjectId, state.Name);
		}
	}
}