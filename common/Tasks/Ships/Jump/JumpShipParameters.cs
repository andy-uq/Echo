using Echo.JumpGates;
using Echo.Ships;

namespace Echo.Tasks.Ships.Jump
{
	public class JumpShipParameters : ITaskParameters
	{
		public JumpShipParameters(Ship ship, JumpGate jumpGate)
		{
			Ship = ship;
			JumpGate = jumpGate;
		}

		public Ship Ship { get; }
		public JumpGate JumpGate { get; }
	}
}