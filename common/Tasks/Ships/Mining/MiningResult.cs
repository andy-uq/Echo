using Echo.Items;
using Echo.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public class MiningResult : TaskResult, ITaskResult
	{
		string ITaskResult.ErrorCode
		{
			get { return ErrorCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return Ship; }
		}

		public MiningTask.ErrorCode ErrorCode { get; set; }
		public Ship Ship { get; set; }

		public Item Ore { get; set; }
	}
}