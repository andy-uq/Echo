﻿using Echo.Builder;
using Echo.Celestial;
using Echo.State;

namespace Echo.JumpGates
{
	partial class JumpGate
	{
		public static class Builder
		{
			public static ObjectBuilder<JumpGate> Build(SolarSystem location, JumpGateState state)
			{
				var jumpGate = new JumpGate
				{
					Id = state.ObjectId,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates)
				};

				return new ObjectBuilder<JumpGate>(jumpGate)
					.Resolve(
						(resolver, target) =>
						target.ConnectsTo = state.ConnectsTo == 0
						                    	? null
						                    	: resolver.GetById<JumpGate>(state.ConnectsTo)
					);
			}
		}
	}
}