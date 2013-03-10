using Echo.Items;

namespace Echo.Tasks.Structure
{
	public class ManufacturingResult : TaskResult, ITaskResult
	{
		private readonly object _errorParams;

		public ManufacturingResult(ManufacturingTask.ErrorCode errorCode)
		{
			Success = (errorCode == ManufacturingTask.ErrorCode.Success);
			ErrorCode = errorCode;
			_errorParams = null;
		}

		public ManufacturingResult()
		{
		}

		public ManufacturingTask.ErrorCode ErrorCode { get; private set; }

		#region ITaskResult Members

		string ITaskResult.ErrorCode
		{
			get { return ErrorCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return _errorParams; }
		}

		public Item Item { get; set; }

		#endregion
	}
}