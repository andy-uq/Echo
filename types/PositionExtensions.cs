using Echo.Celestial;
using Echo.Market;

namespace Echo.Ships
{
	public static class PositionExtensions
	{
		public static Position Leave(this Position position)
		{
			return new Position(null, position.LocalCoordinates);
		}

		public static SolarSystem GetSolarSystem(this Position position)
		{
			while (position.Location != null)
			{
				var solarSystem = position.Location as SolarSystem;
				if (solarSystem != null)
					return solarSystem;

				position = position.Location.Position;
			}

			return null;
		}

		public static MarketPlace GetMarketPlace(this ILocation location)
		{
			while (location != null)
			{
				var starCluster = location as StarCluster;
				if (starCluster != null)
					return starCluster.MarketPlace;

				location = location.Position.Location;
			}

			return null;
		}

		public static MarketPlace GetMarketPlace(this Position position)
		{
			return GetMarketPlace(position.Location);
		}
	}
}