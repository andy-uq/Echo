using System.Collections.Generic;
using Echo.Celestial;
using Echo.State;

namespace Echo.JumpGates
{
	partial class JumpGate
	{
		public class Builder
		{
			public IdResolutionContext<JumpGate> Build(SolarSystem location, JumpGateState state)
			{
				return new IdResolutionContext<JumpGate>
				{
					Target = new JumpGate
					{
						Id = state.Id,
						Name = state.Name,
						Position = new Position(location, state.LocalCoordinates),
					},
					Resolved =
					{
						(resolver, target) => target.ConnectsTo = state.ConnectsTo == -1 ? null : resolver.GetById<JumpGate>(state.ConnectsTo)
					}
				};
			}
		}
	}
}