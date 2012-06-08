using Echo.Builder;
using Echo.State;
using Echo;

namespace Echo.Celestial
{
	partial class CelestialObject
	{
		#region Nested type: Builder

		public class Builder
		{
			public ObjectBuilder<CelestialObject> Build(ILocation target, CelestialObjectState state)
			{
				var builder = Build(state);

				builder.Target.Id = state.ObjectId;
				builder.Target.Name = state.Name;
				builder.Target.Position = new Position(target, state.LocalCoordinates);
				builder.Target.Mass = state.Mass;
				builder.Target.Size = state.Size;

				return builder;
			}

			public CelestialObjectState Save(CelestialObject celestialObject)
			{
				var state = new CelestialObjectState
				{
					ObjectId = celestialObject.Id,
					Name = celestialObject.Name,
					LocalCoordinates = celestialObject.Position.LocalCoordinates,
					Mass = celestialObject.Mass,
					Size = celestialObject.Size
				};

				return Save(celestialObject, state);
			}

			protected virtual CelestialObjectState Save(CelestialObject celestialObject, CelestialObjectState state)
			{
				state.Orbits = celestialObject.AsObjectReference();
				return state;
			}

			protected virtual ObjectBuilder<CelestialObject> Build(CelestialObjectState state)
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
				
				return new ObjectBuilder<CelestialObject>(celestialObject);
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

				protected override ObjectBuilder<CelestialObject> Build(CelestialObjectState state)
				{
					return new ObjectBuilder<CelestialObject>(new AsteroidBelt
					{
						Richness = state.AsteroidBelt.Richness,
						AmountRemaining = state.AsteroidBelt.AmountRemaining
					});
				}
			}

			#endregion

		}

		#endregion
	}
}