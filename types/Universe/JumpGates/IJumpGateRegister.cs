using System.Collections.Generic;

namespace Echo.JumpGates
{
	public interface IJumpGateRegister
	{
		void Register(JumpGate jumpGate);
		void Register(IEnumerable<JumpGate> jumpGates);
		void ResolveGateConnections();
		JumpGate this[long jumpGateId] { get; }
	}
}