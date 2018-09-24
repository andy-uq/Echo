using System;

namespace Echo
{
	public struct Position : IEquatable<Position>
	{
		public ILocation Location { get; }
		public Vector LocalCoordinates { get; }

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

		public Position RotateZ(double radians) => new Position(Location, LocalCoordinates.RotateZ(radians));

		public static Position operator +(Position position, Vector offset) => new Position(position.Location, position.LocalCoordinates + offset);
		public static Position operator -(Position position, Vector offset) => new Position(position.Location, position.LocalCoordinates - offset);
		public static Vector operator -(Position p1, Position p2) => p1.UniversalCoordinates - p2.UniversalCoordinates;

		public static implicit operator Vector(Position p) => p.UniversalCoordinates;

		public bool Equals(Position other) => UniversalCoordinates.Equals(other.UniversalCoordinates);

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Position && Equals((Position) obj);
		}

		public override int GetHashCode()
		{
			return UniversalCoordinates.GetHashCode();
		}

		public static bool operator ==(Position left, Position right) => left.Equals(right);
		public static bool operator !=(Position left, Position right) => !left.Equals(right);

		public override string ToString()
		{
			return Location == null 
				? UniversalCoordinates.ToString()
				: $"{UniversalCoordinates} ({Location})";
		}
	}
}