using EnsureThat;

namespace Echo.Tasks.Structure
{
	public class ManufacturingTask : ITask
	{
		public ITaskResult Execute(ITaskParameters taskParameters)
		{
			var parameters = (ManufacturingParameters)taskParameters;
			return Manufacture(parameters);
		}

		public ManufacturingResult Manufacture(ManufacturingParameters parameters)
		{
			Ensure.That(() => parameters).IsNotNull();

			if ( parameters.BluePrint == null )
				return Failed(ErrorCode.MissingBluePrint);

			if ( parameters.Agent == null )
				return Failed(ErrorCode.MissingAgent);

			if (!parameters.Agent.CanUse(parameters.BluePrint))
				return Failed(ErrorCode.MissingSkillRequirement);

			return Success();
		}

		private ManufacturingResult Success()
		{
			return new ManufacturingResult();
		}

		private ManufacturingResult Failed(ErrorCode errorCode)
		{
			return new ManufacturingResult(errorCode);
		}

		public enum ErrorCode
		{
			Success,
			MissingBluePrint,
			MissingAgent,
			MissingSkillRequirement,
		}
	}
}