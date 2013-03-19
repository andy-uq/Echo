using Echo.Agents;
using Echo.Ships;

namespace Echo.Tasks.Ships.Undocking
{
	public class UndockShipResult : TaskResult, ITaskResult
	{
		public Ship Ship { get; set; }
		public Agent Pilot { get; set; }

		public ShipTask.StatusCode StatusCode { get; set; }

		#region ITaskResult Members

		string ITaskResult.StatusCode
		{
			get { return StatusCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return new {Ship, Pilot}; }
		}

		#endregion
	}
}