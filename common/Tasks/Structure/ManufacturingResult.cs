using Echo.Items;

namespace Echo.Tasks.Structure
{
	public sealed class ManufacturingResult : TaskResult, ITaskResult
	{
		private readonly object _errorParams;

		public ManufacturingResult()
		{
		}

		public ManufacturingResult(ManufacturingTask.StatusCode statusCode)
		{
			Success = (statusCode == ManufacturingTask.StatusCode.Success);
			StatusCode = statusCode;
			_errorParams = null;
		}

		public ManufacturingResult(Item item) : this(ManufacturingTask.StatusCode.Success)
		{
			Item = item;
		}

		public ManufacturingTask.StatusCode StatusCode { get; }
		public Item Item { get; }

		#region ITaskResult Members

		string ITaskResult.StatusCode => StatusCode.ToString();
		object ITaskResult.ErrorParams => _errorParams;

		#endregion
	}
}