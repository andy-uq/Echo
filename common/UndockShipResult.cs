using Echo.Agents;
using Echo.Ships;

namespace Echo
{
	public class UndockShipResult : TaskResult, ITaskResult
	{
		string ITaskResult.ErrorCode
		{
			get { return ErrorCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return new { Ship, Pilot }; }
		}

		public Ship Ship { get; set; }
		public Agent Pilot { get; set; }

		public ShipTask.ErrorCode ErrorCode { get; set; }
	}
}