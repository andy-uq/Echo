using Echo.Agents;
using Echo.Ships;
using Echo.Structures;

namespace Echo
{
	public class UndockShipTask : ShipTask
	{
		private readonly LocationServices _locationServices;

		public UndockShipTask(LocationServices locationServices) : base(locationServices)
		{
			_locationServices = locationServices;
		}

		public TaskResult Execute(Ship ship, Agent pilot)
		{
			var structure = ship.Position.Location as Structure;
			if ( structure == null )
			{
				return Failed(ErrorCode.NotDocked, new { ship });
			}

			if ( !pilot.CanUse(ship) )
			{
				return Failed(ErrorCode.MissingSkillRequirement, new { pilot, ship });
			}

			ship.Position = new Position(structure.Position.Location, _locationServices.GetExitPosition(structure));
			return Success();
		}
	}
}