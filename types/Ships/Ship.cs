using Echo.Celestial;

namespace Echo.Ships
{
	public class Ship : ILocation, IMoves
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Ship; }
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }

		public SolarSystem SolarSystem
		{
			get { return Position.GetSolarSystem(); }
		}

		public void Tick(ulong tick)
		{

		}
	}

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