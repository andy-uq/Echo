using System;
using System.Diagnostics;

using Echo.Objects;
using Echo.Vectors;

namespace Echo.Ships
{
	[DebuggerDisplay("{CurrentPosition}, Equipped: {Weapon}")]
	public class HardPoint
	{
		private readonly Vector origin;

		public double Rotation { get; set; }
		
		/// <summary>Speed of this hard point, measured from 0..1.  0 means the hard point is fixed and cannot move, 1 means the hard point can travel its full rotational degree in one pass</summary>
		public double Speed { get; set; }

		public HardPointPosition Position { get; private set; }

		public Ship Ship { get; private set; }

		public Weapon Weapon { get; private set; }

		public HardPoint(Ship ship, HardPointPosition position) : this(position)
		{
			Ship = ship;
		}

		public static HardPoint FactoryHardPoint(HardPointPosition position)
		{
			return new HardPoint(position);
		}

		private HardPoint(HardPointPosition position)
		{
			Position = position;
			Speed = 0.5d;

			switch (Position)
			{
				case HardPointPosition.Front:
					this.origin = new Vector(0, 1, 0);
					Rotation = Math.PI;
					break;
				case HardPointPosition.Rear:
					this.origin = new Vector(0, -1, 0);
					Rotation = Math.PI;
					break;
				case HardPointPosition.Left:
					this.origin = new Vector(-1, 0, 0);
					Rotation = Math.PI / 2;
					break;
				case HardPointPosition.Right:
					this.origin = new Vector(1, 0, 0);
					Rotation = Math.PI / 2;
					break;
				case HardPointPosition.Top:
					this.origin = new Vector(0, 1, 0);
					Rotation = Math.PI * 2;
					break;
				case HardPointPosition.Bottom:
					this.origin = new Vector(0, 1, 0);
					Rotation = Math.PI * 2;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Orientation = this.origin;
		}

		public double Inclination
		{
			get
			{
				Vector rightExtent = origin.RotateZ(Rotation/-2d);

				double radians = Vector.Angle(rightExtent, Orientation) - (Rotation / 2d);
				return Math.Round(radians*(0.5/Math.PI)*360, 4);
			}
		}

		public void EquipWeapon(Weapon weapon)
		{
			Weapon = weapon;
			Weapon.Location = Ship;
		}

		public Vector Orientation
		{
			get; set;
		}

		public Vector Origin
		{
			get { return this.origin; }
		}

		public bool AimAt(ILocation target)
		{
			Vector targetPosition = (target.UniversalCoordinates - Ship.UniversalCoordinates);
			targetPosition = targetPosition.ToUnitVector();

			double extent = Vector.Angle(origin, targetPosition);
			double angleToMove = Vector.Angle(Orientation, targetPosition);

			if (extent - (Rotation / 2) > 0.0001)
			{
				angleToMove = Math.Min(Rotation * Speed, Rotation / 2);
				Vector counter = origin.RotateZ(angleToMove);
				Vector clock = origin.RotateZ(angleToMove*-1d);

				Orientation = (counter - targetPosition).Magnitude < (clock - targetPosition).Magnitude ? counter : clock;
				return false;
			}

			if ( angleToMove - (Rotation * Speed) > 0.0001)
			{
				angleToMove = Rotation * Speed;
				Vector counter = Orientation.RotateZ(angleToMove);
				Vector clock = Orientation.RotateZ(angleToMove * -1d);

				Orientation = (counter - targetPosition).Magnitude < (clock - targetPosition).Magnitude ? counter : clock;
				return false;
			}

			Orientation = targetPosition;
			return true;
		}

		public bool InRange(ILocation target)
		{
			Vector targetPosition = (target.UniversalCoordinates - Ship.UniversalCoordinates);
			targetPosition = targetPosition.ToUnitVector();

			double extent = Vector.Angle(origin, targetPosition);
			return (extent - (Rotation / 2) < 0.0001);
		}

		public bool CanTrack(ILocation target)
		{
			Vector targetPosition = (target.UniversalCoordinates - Ship.UniversalCoordinates);
			targetPosition = targetPosition.ToUnitVector();

			double extent = Math.Acos(origin.DotProduct(targetPosition));
			double angleToMove = Vector.Angle(Orientation, targetPosition);

			if ( extent - (Rotation / 2) > 0.0001 )
				return false;

			if ( angleToMove - (Rotation * Speed) > 0.0001 )
				return false;

			return true;
		}

		public void Reset()
		{
			Orientation = this.origin;
		}

		public HardPoint Clone()
		{
			if (Weapon != null)
				throw new InvalidOperationException("Cannot clone this hard point because it is not a factory hard-point.");

			var clone = (HardPoint) MemberwiseClone();
			return clone;
		}
	}
}