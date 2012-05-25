using Echo.Celestial;

namespace Echo.Ships
{
	public static class PositionExtensions
	{
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
	}
}