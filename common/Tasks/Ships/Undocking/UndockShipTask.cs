using Echo.Agents;
using Echo.Ships;

namespace Echo.Tasks.Ships.Undocking
{
	public class UndockShipTask : ShipTask<UndockShipParameters, UndockShipResult>
	{
		private readonly ILocationServices _locationServices;

		public UndockShipTask(ILocationServices locationServices)
		{
			_locationServices = locationServices;
		}

		public override UndockShipResult Execute(UndockShipParameters undockShipParameters)
		{
			var structure = undockShipParameters.Ship.Position.Location as Structures.Structure;
			if (structure == null)
			{
				return Failed(ErrorCode.NotDocked, undockShipParameters.Ship);
			}

			if (!undockShipParameters.Pilot.CanUse(undockShipParameters.Ship))
			{
				return Failed(ErrorCode.MissingSkillRequirement, undockShipParameters.Ship, undockShipParameters.Pilot);
			}

			undockShipParameters.Ship.Position = new Position(structure.Position.Location,
			                                                  _locationServices.GetExitPosition(structure));
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