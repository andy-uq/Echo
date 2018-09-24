using Echo.Agents;
using Echo.JumpGates;
using Echo.Ships;

namespace Echo.Tasks.Ships.Jump
{
	public class JumpShipTask : ShipTask<JumpShipParameters, JumpShipResult>
	{
		private readonly ILocationServices _locationServices;

		public JumpShipTask(ILocationServices locationServices)
		{
			_locationServices = locationServices;
		}

		private Ship Ship { get; set; }
		private Agent Pilot => Ship.Pilot;
		private JumpGate JumpGate { get; set; }

		protected override JumpShipResult SetParameters(JumpShipParameters parameters)
		{
			Ship = parameters.Ship;
			JumpGate = parameters.JumpGate;

			return ValidateParameters();
		}

		public override ITaskResult Execute()
		{
			var result = ValidateParameters();
			if (result.Success)
			{
				var exitPosition = _locationServices.GetExitPosition(JumpGate.ConnectsTo);
				Ship.Position = new Position(JumpGate.Position.Location, exitPosition);
			}

			return result;
		}

		private JumpShipResult ValidateParameters()
		{
			if (Ship.Pilot == null)
			{
				return Failed(StatusCode.NoPilot);
			}

			if (JumpGate.OutOfRange(Ship))
			{
				return Failed(StatusCode.NotInPosition);
			}

			return Success();
		}

		private JumpShipResult Failed(StatusCode statusCode)
		{
			return new JumpShipResult
			{
				StatusCode = statusCode,
				Ship = Ship,
				Pilot = Pilot,
				JumpGate = JumpGate
			};
		}
	}
}