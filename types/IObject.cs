using Echo;
using Echo.State;
using EnsureThat;

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