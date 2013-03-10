using System;

namespace Echo.Tasks.Ships
{
	public abstract class ShipTask : ITask
	{
		#region ErrorCode enum

		public enum ErrorCode
		{
			Success,
			NotDocked,
			MissingSkillRequirement
		}

		#endregion

		#region ITask Members

		public abstract ITaskResult Execute(ITaskParameters taskParameters);

		#endregion
	}

	public abstract class ShipTask<TParameters, TResult> : ShipTask
		where TParameters : ITaskParameters
		where TResult : TaskResult, ITaskResult, new()
	{
		public override ITaskResult Execute(ITaskParameters taskParameters)
		{
			return Execute((TParameters) taskParameters);
		}

		public abstract TResult Execute(TParameters parameters);

		protected TResult Success(Func<TResult> taskResult = null)
		{
			if (taskResult == null)
			{
				return new TResult {Success = true};
			}

			TResult result = taskResult();
			result.Success = true;

			return result;
		}
	}
}