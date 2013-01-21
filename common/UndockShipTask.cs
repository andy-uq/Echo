using Echo.Agents;
using Echo.Ships;
using Echo.Structures;

namespace Echo
{
	public class UndockShipTask : ShipTask<UndockShipResult>
	{
		private readonly ILocationServices _locationServices;

		public UndockShipTask(ILocationServices locationServices) : base(locationServices)
		{
			_locationServices = locationServices;
		}

		public UndockShipResult Execute(Ship ship, Agent pilot)
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

		private UndockShipResult Failed(ErrorCode errorCode, Ship ship, Agent pilot = null)
		{
			return new UndockShipResult
			{
				ErrorCode = errorCode,
				Ship = ship,
				Pilot = pilot,
			};
		}
	}
}