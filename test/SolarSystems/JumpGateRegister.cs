using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Exceptions;
using Echo.JumpGates;
using Echo.State;

namespace Echo.Tests.SolarSystems
{
	public class JumpGateRegister : IIdResolver
	{
		private readonly Dictionary<ulong, JumpGate> _registry = new Dictionary<ulong, JumpGate>();

		public void Register(JumpGate jumpGate)
		{
			_registry.Add(jumpGate.Id, jumpGate);
		}

		public void Register(IEnumerable<JumpGate> jumpGates)
		{
			foreach (var x in jumpGates)
				Register(x);
		}

		public IEnumerable<IObject> Values => _registry.Values.ToArray();

		public T GetById<T>(ulong id) where T : class, IObject
		{
			if (typeof(T) == typeof(JumpGate))
			{
				if (_registry.TryGetValue(id, out var jumpGate))
					return jumpGate as T;

				throw new ItemNotFoundException("Jump Gate", id);
			}

			throw new ArgumentException("Cannot resolve objects that are not jump gates");
		}

		public bool TryGetById<T>(ulong id, out T value) where T : class, IObject
		{
			if (_registry.TryGetValue(id, out var jumpGate))
			{
				value = jumpGate as T;
				return value != null;
			}

			value = null;
			return false;
		}

		public T Get<T>(ObjectReference objectReference) where T : class, IObject
		{
			throw new NotSupportedException();
		}

		public bool TryGet<T>(ObjectReference objectReference, out T value) where T : class, IObject
		{
			throw new NotSupportedException();
		}

		public T Get<T>(ObjectReference? objectReference) where T : class, IObject
		{
			throw new NotSupportedException();
		}

		public bool TryGet<T>(ObjectReference? objectReference, out T value) where T : class, IObject
		{
			throw new NotSupportedException();
		}
	}
}