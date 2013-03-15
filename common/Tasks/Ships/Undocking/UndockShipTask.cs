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

		private Ship Ship { get; set; }
		private Agent Pilot { get; set; }
		private Structures.Structure Structure { get; set; }

		protected override UndockShipResult SetParameters(UndockShipParameters parameters)
		{
			Pilot = parameters.Pilot;
			Ship = parameters.Ship;
			Structure = Ship.Position.Location as Structures.Structure;

			return ValidateParameters();
		}

		public override ITaskResult Execute()
		{
			var result = ValidateParameters();
			if ( result.Success )
			{
				var exitPosition = _locationServices.GetExitPosition(Structure);
				Ship.Position = new Position(Structure.Position.Location, exitPosition);
			}

			return result;
		}
		
		private UndockShipResult ValidateParameters()
		{
			if ( Structure == null )
			{
				{
					return Failed(ErrorCode.NotDocked, Ship);
				}
			}

			if ( !Pilot.CanUse(Ship) )
			{
				return Failed(ErrorCode.MissingSkillRequirement, Ship, Pilot);
			}

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