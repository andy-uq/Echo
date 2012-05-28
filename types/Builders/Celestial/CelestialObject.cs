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

			public CelestialObjectState Save(CelestialObject celestialObject)
			{
				var state = new CelestialObjectState
				{
					Id = celestialObject.Id,
					Name = celestialObject.Name,
					LocalCoordinates = celestialObject.Position.LocalCoordinates,
					Mass = celestialObject.Mass,
					Size = celestialObject.Size
				};

				return Save(celestialObject, state);
			}

			protected virtual CelestialObjectState Save(CelestialObject celestialObject, CelestialObjectState state)
			{
				return state;
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

			public static Builder For(CelestialObject celestialObject)
			{
				if (celestialObject is AsteroidBelt)
					return new AsteroidBeltBuilder();
				
				return new Builder();
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
				protected override CelestialObjectState Save(CelestialObject celestialObject, CelestialObjectState state)
				{
					var asteroidBelt = (AsteroidBelt) celestialObject;
					state.AsteroidBelt = new AsteroidBeltState
					{
						AmountRemaining = asteroidBelt.AmountRemaining,
						Richness = asteroidBelt.Richness,
					};

					return state;
				}

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