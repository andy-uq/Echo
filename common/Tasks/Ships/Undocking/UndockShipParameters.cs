using Echo.Agents;
using Echo.Ships;

namespace Echo.Tasks.Ships.Undocking
{
	public class UndockShipParameters : ITaskParameters
	{
		private readonly Agent _pilot;
		private readonly Ship _ship;

		public UndockShipParameters(Ship ship, Agent pilot)
		{
			_ship = ship;
			_pilot = pilot;
		}

		public Ship Ship
		{
			get { return _ship; }
		}

		public Agent Pilot
		{
			get { return _pilot; }
		}
	}
}