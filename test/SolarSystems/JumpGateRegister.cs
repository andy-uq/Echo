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
		private readonly Dictionary<long, JumpGate> _registry = new Dictionary<long, JumpGate>();

		public void Register(JumpGate jumpGate)
		{
			_registry.Add(jumpGate.Id, jumpGate);
		}

		public void Register(IEnumerable<JumpGate> jumpGates)
		{
			foreach (var x in jumpGates)
				Register(x);
		}

		public IEnumerable<IObject> Values
		{
			get { return _registry.Values.ToArray(); }
		}

		public T GetById<T>(long id) where T : class, IObject
		{
			if (typeof(T) == typeof(JumpGate))
			{
				JumpGate jumpGate;
				if (_registry.TryGetValue(id, out jumpGate))
					return jumpGate as T;

				throw new ItemNotFoundException("Jump Gate", id);
			}

			throw new ArgumentException("Cannot resolve objects that are not jump gates");
		}

		public bool TryGetById<T>(long id, out T value) where T : class, IObject
		{
			JumpGate jumpGate;
			if (!_registry.TryGetValue(id, out jumpGate))
			{
				jumpGate = null;
			}

			value = jumpGate as T;
			return value != null;
		}

		public T Get<T>(ObjectReference objectReference) where T : class, IObject
		{
			throw new NotImplementedException();
		}

		public bool TryGet<T>(ObjectReference objectReference, out T value) where T : class, IObject
		{
			throw new NotImplementedException();
		}

		public T Get<T>(ObjectReference? objectReference) where T : class, IObject
		{
			throw new NotImplementedException();
		}

		public bool TryGet<T>(ObjectReference? objectReference, out T value) where T : class, IObject
		{
			throw new NotImplementedException();
		}
	}
}