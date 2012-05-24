using Echo.State;
using Echo;

namespace Echo.Celestial
{
	partial class CelestialObject
	{
		#region Nested type: Builder

		public class Builder
		{
			public CelestialObject Build(ILocation target, CelestialObjectState state)
			{
				CelestialObject celestialObject = Build(state);

				celestialObject.Id = state.Id;
				celestialObject.Name = state.Name;
				celestialObject.Position = new Position(target, state.LocalCoordinates);
				celestialObject.Mass = state.Mass;
				celestialObject.Size = state.Size;

				return celestialObject;
			}

			protected virtual CelestialObject Build(CelestialObjectState state)
			{
				CelestialObject celestialObject;

				switch (state.CelestialObjectType)
				{
					case CelestialObjectType.Planet:
						celestialObject = new Planet();
						break;
					case CelestialObjectType.Moon:
						celestialObject = new Moon();
						break;

					case CelestialObjectType.Object:
					default:
						celestialObject = new CelestialObject();
						break;
				}
				return celestialObject;
			}

			public static Builder For(CelestialObjectState state)
			{
				switch (state.CelestialObjectType)
				{
					case CelestialObjectType.AsteriodBelt:
						return new AsteroidBeltBuilder();

					default:
						return new Builder();
				}
			}

			#region Nested type: AsteroidBeltBuilder

			private class AsteroidBeltBuilder : Builder
			{
				protected override CelestialObject Build(CelestialObjectState state)
				{
					return new AsteroidBelt
					{
						Richness = state.AsteroidBelt.Richness,
						AmountRemaining = state.AsteroidBelt.AmountRemaining
					};
				}
			}

			#endregion
		}

		#endregion
	}
}