using Echo.JumpGates;
using Echo.Ships;

namespace Echo.Tasks.Ships.Jump
{
	public class JumpShipParameters : ITaskParameters
	{
		private readonly Ship _ship;
		private readonly JumpGate _jumpGate;

		public JumpShipParameters(Ship ship, JumpGate jumpGate)
		{
			_ship = ship;
			_jumpGate = jumpGate;
		}

		public Ship Ship
		{
			get { return _ship; }
		}

		public JumpGate JumpGate
		{
			get { return _jumpGate; }
		}
	}
}