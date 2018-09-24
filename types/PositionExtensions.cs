using Echo.Celestial;
using Echo.Market;

namespace Echo
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
				if (position.Location is SolarSystem solarSystem)
					return solarSystem;

				position = position.Location.Position;
			}

			return null;
		}

		public static StarCluster GetStarCluster(this ILocation location)
		{
			while (location != null)
			{
				if (location is StarCluster starCluster)
					return starCluster;

				location = location.Position.Location;
			}

			return null;
		}

		public static MarketPlace GetMarketPlace(this ILocation location)
		{
			var starCluster = location.GetStarCluster();
			return starCluster?.MarketPlace;
		}

		public static MarketPlace GetMarketPlace(this Position position)
		{
			return GetMarketPlace(position.Location);
		}
	}
}