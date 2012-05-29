using System.Collections.Generic;

namespace Echo.JumpGates
{
	public interface IJumpGateResolver
	{
		void Register(JumpGate jumpGate);
		void Register(IEnumerable<JumpGate> jumpGates);
		void Resolve();
	}
}