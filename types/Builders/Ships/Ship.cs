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

			public static Ship Build(ILocation location, ShipState state)
			{
				var ship = new Ship
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates),
				};

				ship._hardPoints = state.HardPoints.Build(ship).ToList();
				return ship;
			}
		}
	}
}