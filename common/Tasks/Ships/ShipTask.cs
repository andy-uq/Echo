using System;

namespace Echo.Tasks.Ships
{
	public abstract class ShipTask : ITask
	{
		#region StatusCode enum

		public enum StatusCode
		{
			Success,
			NotDocked,
			MissingSkillRequirement,
			Pending
		}

		#endregion

		#region ITask Members

		public abstract ITaskResult SetParameters(ITaskParameters taskParameters);
		public abstract ITaskResult Execute();

		#endregion
	}

	public abstract class ShipTask<TParameters, TResult> : ShipTask
		where TParameters : ITaskParameters
		where TResult : TaskResult, ITaskResult, new()
	{
		public override ITaskResult SetParameters(ITaskParameters taskParameters)
		{
			return SetParameters((TParameters) taskParameters);
		}

		protected abstract TResult SetParameters(TParameters parameters);

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