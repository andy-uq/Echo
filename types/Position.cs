namespace Echo
{
	public struct Position
	{
		public ILocation Location { get; private set; }
		public Vector LocalCoordinates { get; private set; }

		public Position(ILocation location, Vector localCoordinates) : this()
		{
			Location = location;
			LocalCoordinates = localCoordinates;
		}

		public Vector UniversalCoordinates
		{
			get
			{
				if (Location == null)
					return LocalCoordinates;

				return Location.Position.UniversalCoordinates + LocalCoordinates;
			}
		}
	}
}