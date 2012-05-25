using System.Collections.Generic;
using Echo.Exceptions;

namespace Echo.JumpGates
{
	public class JumpGateRegister : IJumpGateRegister
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

		public JumpGate this[long id]
		{
			get
			{
				JumpGate value;
				if (_registry.TryGetValue(id, out value))
					return value;

				throw new ItemNotFoundException("Jump Gate", id);
			}
		}

		public void ResolveGateConnections()
		{
			foreach (var jumpGate in _registry.Values)
				((IJumpGateResolver )jumpGate).ResolveConnectedGate(this);
		}
	}
}