using Echo.Agents;
using Echo.Ships;
using Echo.Structures;

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

		public TaskResult Undock(Ship ship, Agent pilot)
		{
			var structure = ship.Position.Location as Structure;
			if (structure == null)
			{
				return Failed(ErrorCode.NotDocked, new {ship});
			}

			if (!pilot.CanUse(ship))
			{
				return Failed(ErrorCode.MissingSkillRequirement, new {pilot, ship});
			}

			ship.Position = new Position(structure.Position.Location, _locationServices.GetExitPosition(structure));
			return Success();
		}

		private TaskResult Success()
		{
			return new TaskResult
			{
				Success = true
			};
		}

		private TaskResult Failed(ErrorCode errorCode, object errorParams)
		{
			return new TaskResult
			{
				Success = false,
				ErrorCode = errorCode.ToString(),
				ErrorParams = errorParams
			};
		}
	}

	public class TaskResult
	{
		public string Task { get; set; }

		public bool Success { get; set; }
		public string ErrorCode { get; set; }
		public object ErrorParams { get; set; }
	}
}