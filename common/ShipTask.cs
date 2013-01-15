namespace Echo
{
	public abstract class ShipTask
	{
		private readonly ILocationServices _locationServices;

		public enum ErrorCode
		{
			Success,
			NotDocked,
			MissingSkillRequirement
		}

		public ILocationServices LocationServices
		{
			get { return _locationServices; }
		}

		protected ShipTask(ILocationServices locationServices)
		{
			_locationServices = locationServices;
		}
	}

	public abstract class ShipTask<TTaskResult> : ShipTask 
		where TTaskResult : TaskResult, ITaskResult, new()
	{
		protected ShipTask(ILocationServices locationServices) : base(locationServices)
		{
		}

		protected TTaskResult Success()
		{
			return new TTaskResult { Success = true };
		}
	}
}