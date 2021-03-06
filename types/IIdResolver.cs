using System.Collections.Generic;
using Echo.State;

namespace Echo
{
	public interface IIdResolver
	{
		IEnumerable<IObject> Values { get; }

		T GetById<T>(ulong id) where T : class, IObject;
		bool TryGetById<T>(ulong id, out T value) where T : class, IObject;
		T Get<T>(ObjectReference objectReference) where T : class, IObject;
		bool TryGet<T>(ObjectReference objectReference, out T value) where T : class, IObject;
		T Get<T>(ObjectReference? objectReference) where T : class, IObject;
		bool TryGet<T>(ObjectReference? objectReference, out T value) where T : class, IObject;
	}
}