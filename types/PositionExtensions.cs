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

		public static MarketPlace GetMarketPlace(this Position position)
		{
			while (position.Location != null)
			{
				var starCluster = position.Location as StarCluster;
				if (starCluster != null)
					return starCluster.MarketPlace;

				position = position.Location.Position;
			}

			return null;
		}
	}
}