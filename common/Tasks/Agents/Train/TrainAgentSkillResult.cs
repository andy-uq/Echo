namespace Echo.Tasks.Agents.Train
{
	public class TrainAgentSkillResult : TaskResult, ITaskResult
	{
		private readonly object _errorParams;

		public TrainAgentSkillResult()
		{
		}

		public TrainAgentSkillResult(TrainAgentSkillTask.StatusCode statusCode)
		{
			Success = (statusCode == TrainAgentSkillTask.StatusCode.Success);
			StatusCode = statusCode;
			_errorParams = null;
		}

		public TrainAgentSkillTask.StatusCode StatusCode { get; }

		string ITaskResult.StatusCode => StatusCode.ToString();
		object ITaskResult.ErrorParams => _errorParams;
	}
}