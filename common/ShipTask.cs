namespace Echo
{
	public abstract class ShipTask
	{
		public enum ErrorCode
		{
			Success,
			NotDocked,
			MissingSkillRequirement
		}
	}

	public abstract class ShipTask<TTaskResult> : ShipTask 
		where TTaskResult : TaskResult, ITaskResult, new()
	{
		protected TTaskResult Success()
		{
			return new TTaskResult { Success = true };
		}
	}
}