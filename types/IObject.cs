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
				return null;

			return new ObjectReference { Id = @object.Id, Name = @object.Name };
		}

		public static ObjectReference AsObjectReference(this IObjectState state)
		{
			if ( state == null )
				return null;

			return new ObjectReference { Id = state.Id, Name = state.Name };
		}
	}
}