﻿using System;

namespace Echo.Tasks.Ships
{
	public abstract class ShipTask : ITask
	{
		#region StatusCode enum

		public enum StatusCode
		{
			Success,
			NoPilot,
			NotDocked,
			NotInPosition,
			MissingSkillRequirement,
			Pending
		}

		#endregion

		#region ITask Members

		public abstract ITaskResult SetParameters(ITaskParameters taskParameters);
		public abstract ITaskResult Execute();

		#endregion

		public ulong Id { get; set; }
		public string Name { get; set; }
		public ObjectType ObjectType => ObjectType.Task;
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

			var result = taskResult();
			result.Success = true;

			return result;
		}
	}
}