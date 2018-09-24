using Echo.Agents;
using Echo.JumpGates;
using Echo.Ships;

namespace Echo.Tasks.Ships.Jump
{
	public class JumpShipResult : TaskResult, ITaskResult
	{
		public Ship Ship { get; set; }
		public Agent Pilot { get; set; }
		public JumpGate JumpGate { get; set; }

		public ShipTask.StatusCode StatusCode { get; set; }

		#region ITaskResult Members

		string ITaskResult.StatusCode => StatusCode.ToString();

		object ITaskResult.ErrorParams => new { Ship, Pilot, JumpGate };

		#endregion
	}
}