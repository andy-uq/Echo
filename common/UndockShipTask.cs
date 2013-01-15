using Echo.Agents;
using Echo.Ships;
using Echo.Structures;

namespace Echo
{
	public class UndockShipTaskResult : TaskResult, ITaskResult
	{
		string ITaskResult.ErrorCode
		{
			get { return ErrorCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return new {Ship, Pilot}; }
		}

		public Ship Ship { get; set; }
		public Agent Pilot { get; set; }

		public ShipTask.ErrorCode ErrorCode { get; set; }
	}

	public class UndockShipTask : ShipTask<UndockShipTaskResult>
	{
		private readonly ILocationServices _locationServices;

		public UndockShipTask(ILocationServices locationServices) : base(locationServices)
		{
			_locationServices = locationServices;
		}

		public UndockShipTaskResult Execute(Ship ship, Agent pilot)
		{
			var structure = ship.Position.Location as Structure;
			if ( structure == null )
			{
				return Failed(ErrorCode.NotDocked, ship);
			}

			if ( !pilot.CanUse(ship) )
			{
				return Failed(ErrorCode.MissingSkillRequirement, ship, pilot);
			}

			ship.Position = new Position(structure.Position.Location, _locationServices.GetExitPosition(structure));
			return Success();
		}

		private UndockShipTaskResult Failed(ErrorCode errorCode, Ship ship, Agent pilot = null)
		{
			return new UndockShipTaskResult
			{
				ErrorCode = errorCode,
				Ship = ship,
				Pilot = pilot,
			};
		}
	}
}