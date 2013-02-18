using System;

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
		protected TTaskResult Success(Func<TTaskResult> taskResult = null)
		{
			if ( taskResult == null )
			{
				return new TTaskResult {Success = true};
			}

			var result = taskResult();
			result.Success = true;
			
			return result;
		}
	}
}