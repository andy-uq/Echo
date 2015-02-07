using System;

namespace Echo
{
	public struct Position : IEquatable<Position>
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

		public static Position operator +(Position position, Vector offset)
		{
			return new Position(position.Location, position.LocalCoordinates + offset);
		}

		public static Position operator -(Position position, Vector offset)
		{
			return new Position(position.Location, position.LocalCoordinates - offset);
		}

		public bool Equals(Position other)
		{
			return UniversalCoordinates.Equals(other.UniversalCoordinates);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Position && Equals((Position) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Location.GetHashCode()*397) ^ LocalCoordinates.GetHashCode();
			}
		}

		public static bool operator ==(Position left, Position right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Position left, Position right)
		{
			return !left.Equals(right);
		}
	}
}