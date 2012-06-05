using System.Collections.Generic;
using System.Linq;
using Echo.Builders;
using Echo.State;

namespace Echo.Ships
{
	partial class Ship
	{
		public static class Builder
		{
			public static ShipState Save(Ship ship)
			{
				return new ShipState
				{
					Id = ship.Id,
					Name = ship.Name,
					LocalCoordinates = ship.Position.LocalCoordinates,
					HardPoints = ship.HardPoints.Save()
				};
			}

			public static ObjectBuilder<Ship> Build(ILocation location, ShipState state)
			{
				var ship = new Ship
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates)
				};

				var builder = new ObjectBuilder<Ship>(ship)
				{
					Connect =
						{
							(resolver, target) => BuildHardPoints(ship, state.HardPoints)
						}
				};

				return builder;
			}

			private static void BuildHardPoints(Ship ship, IEnumerable<HardPointState> hardPoints)
			{
				ship._hardPoints = (hardPoints ?? Enumerable.Empty<HardPointState>()).Select(h => HardPoint.Builder.Build(ship, h)).ToList();
			}
		}
	}
}