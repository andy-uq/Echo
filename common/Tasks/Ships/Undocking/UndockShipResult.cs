using Echo.Agents;
using Echo.Ships;

namespace Echo.Tasks.Ships.Undocking
{
	public sealed class UndockShipResult : TaskResult, ITaskResult
	{
		public Ship Ship { get; set; }
		public Agent Pilot { get; set; }

		public ShipTask.StatusCode StatusCode { get; set; }

		#region ITaskResult Members

		string ITaskResult.StatusCode => StatusCode.ToString();
		object ITaskResult.ErrorParams => new {Ship, Pilot};

		#endregion
	}
}