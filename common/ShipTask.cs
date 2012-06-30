namespace Echo
{
	public class ShipTask
	{
		private readonly LocationServices _locationServices;

		public enum ErrorCode
		{
			Success,
			NotDocked,
			MissingSkillRequirement
		}

		public ShipTask(LocationServices locationServices)
		{
			_locationServices = locationServices;
		}

		protected TaskResult Success()
		{
			return new TaskResult
			{
				Success = true
			};
		}

		protected TaskResult Failed(ErrorCode errorCode, object errorParams)
		{
			return new TaskResult
			{
				Success = false,
				ErrorCode = errorCode.ToString(),
				ErrorParams = errorParams
			};
		}
	}
}