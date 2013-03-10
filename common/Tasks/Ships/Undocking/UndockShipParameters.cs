using Echo.Agents;

namespace Echo.Tasks.Ships.Undocking
{
	public class UndockShipParameters : ITaskParameters
	{
		private readonly Echo.Ships.Ship _ship;
		private readonly Agent _pilot;

		public UndockShipParameters(Echo.Ships.Ship ship, Agent pilot)
		{
			_ship = ship;
			_pilot = pilot;
		}

		public Echo.Ships.Ship Ship
		{
			get { return _ship; }
		}

		public Agent Pilot
		{
			get { return _pilot; }
		}
	}
}