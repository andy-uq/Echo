using Echo.Agents;
using Echo.Ships;

namespace Echo.Tasks.Ships.Undocking
{
	public class UndockShipParameters : ITaskParameters
	{
		public UndockShipParameters(Ship ship, Agent pilot)
		{
			Ship = ship;
			Pilot = pilot;
		}

		public Ship Ship { get; }
		public Agent Pilot { get; }
	}
}