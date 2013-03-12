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
			protected CelestialObjectState State { get; set; }

			protected Builder()
			{}

			protected Builder(CelestialObjectState state)
			{
				State = state;
			}

			public ObjectBuilder<CelestialObject> Build(ILocation target)
			{
				var builder = Build();

				builder.Target.Id = State.ObjectId;
				builder.Target.Name = State.Name;
				builder.Target.Position = new Position(target, State.LocalCoordinates);
				builder.Target.Mass = State.Mass;
				builder.Target.Size = State.Size;

				return builder;
			}

			public CelestialObjectState Save(CelestialObject celestialObject)
			{
				State = new CelestialObjectState
				{
					ObjectId = celestialObject.Id,
					Name = celestialObject.Name,
					CelestialObjectType = celestialObject.CelestialObjectType,
					LocalCoordinates = celestialObject.Position.LocalCoordinates,
					Orbits = celestialObject.Position.Location.AsObjectReference(),
					Mass = celestialObject.Mass,
					Size = celestialObject.Size
				};

				return SaveCelestialObject(celestialObject);
			}

			protected virtual CelestialObjectState SaveCelestialObject(CelestialObject celestialObject)
			{
				return State;
			}

			protected virtual ObjectBuilder<CelestialObject> Build()
			{
				CelestialObject celestialObject;

				switch (State.CelestialObjectType)
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
				
				if (celestialObject is Planet)
					return new PlanetBuilder();

				return new Builder();
			}

			public static Builder For(CelestialObjectState state)
			{
				switch (state.CelestialObjectType)
				{
					case CelestialObjectType.AsteriodBelt:
						return new AsteroidBeltBuilder(state);

					default:
						return new Builder(state);
				}
			}

			private class PlanetBuilder : Builder
			{
				protected override CelestialObjectState SaveCelestialObject(CelestialObject celestialObject)
				{
					var state = base.SaveCelestialObject(celestialObject);
					state.Orbits = null;

					return state;
				}
			}

			#region Nested type: AsteroidBeltBuilder

			private class AsteroidBeltBuilder : Builder
			{
				public AsteroidBeltBuilder(CelestialObjectState state) : base(state)
				{	
				}

				public AsteroidBeltBuilder()
				{
				}

				protected override CelestialObjectState SaveCelestialObject(CelestialObject celestialObject)
				{
					var asteroidBelt = (AsteroidBelt) celestialObject;

					var state = base.SaveCelestialObject(asteroidBelt);					
					state.AsteroidBelt = new AsteroidBeltState
					{
						AmountRemaining = asteroidBelt.AmountRemaining,
						Richness = asteroidBelt.Richness,
					};

					return state;
				}

				protected override ObjectBuilder<CelestialObject> Build()
				{
					return new ObjectBuilder<CelestialObject>(new AsteroidBelt
					{
						Ore = State.AsteroidBelt.Ore,
						Richness = State.AsteroidBelt.Richness,
						AmountRemaining = State.AsteroidBelt.AmountRemaining
					});
				}
			}

			#endregion

		}

		#endregion
	}
}