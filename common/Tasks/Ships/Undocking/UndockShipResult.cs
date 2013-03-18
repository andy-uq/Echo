using Echo.Agents;
using Echo.Ships;

namespace Echo.Tasks.Ships.Undocking
{
	public class UndockShipResult : TaskResult, ITaskResult
	{
		public Ship Ship { get; set; }
		public Agent Pilot { get; set; }

		public ShipTask.ErrorCode ErrorCode { get; set; }

		#region ITaskResult Members

		string ITaskResult.StatusCode
		{
			get { return ErrorCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return new {Ship, Pilot}; }
		}

		#endregion
	}
}