using Echo.Items;
using Echo.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public sealed class MiningResult : TaskResult, ITaskResult
	{
		public ShipTask.StatusCode StatusCode { get; set; }
		public Ship Ship { get; set; }

		public Item Ore { get; set; }

		#region ITaskResult Members

		string ITaskResult.StatusCode => StatusCode.ToString();

		object ITaskResult.ErrorParams => Ship;

		#endregion
	}
}