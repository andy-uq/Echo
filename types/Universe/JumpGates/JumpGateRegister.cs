﻿using System;
using System.Collections.Generic;
using Echo.Exceptions;

namespace Echo.JumpGates
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
	}
}