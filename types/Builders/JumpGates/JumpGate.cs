using System.Collections.Generic;
using Echo.Celestial;
using Echo.State;

namespace Echo.JumpGates
{
	partial class JumpGate
	{
		public class Builder
		{
			public JumpGate Build(SolarSystem location, JumpGateState state)
			{
				var jumpGate = new JumpGate
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates),
					_connectsToJumpGateId = state.ConnectsTo
				};
			
				return jumpGate;
			}
		}
	}
}