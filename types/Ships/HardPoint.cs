using System;
using System.Diagnostics;
using EnsureThat;

namespace Echo.Ships
{
	[DebuggerDisplay("{CurrentPosition}, Equipped: {Weapon}")]
	public partial class HardPoint
	{
		private readonly Vector _origin;
		private readonly double _radiansOfMovement;

		private Vector _orientation;

		public HardPoint(Ship ship, HardPointPosition position)
			: this(position)
		{
			Ship = ship;
		}

		private HardPoint(HardPointPosition position)
		{
			Position = position;
			Speed = 0.5d;

			CalculateHardPoint(position, out _origin, out _radiansOfMovement);
			Orientation = _origin;
		}

		public static Vector CalculateOrientation(HardPointPosition position)
		{
			double radiansOfMovement;
			Vector origin;

			CalculateHardPoint(position, out origin, out radiansOfMovement);
			return origin;
		}

		/// <summary>Calculate the absolute orientation of the hard point (relative to the ship heading)</summary>
		/// <param name="orientation"> </param>
		/// <returns></returns>
		private Vector GetAbsoluteOrientation(Vector orientation)
		{
			if (Ship.Heading == Vector.Zero)
				return orientation;

			var upVector = new Vector(0, 1);

			var rightVector = (Ship.Heading * upVector);

			var rotate = Vector.Angle(upVector, Ship.Heading);
			return rightVector.Z < 0d 
				? orientation.RotateZ(rotate) 
				: orientation.RotateZ(-rotate);
		}

		public static void CalculateHardPoint(HardPointPosition position, out Vector origin, out double radiansOfMovement)
		{
			switch ( position )
			{
				case HardPointPosition.Front:
					origin = new Vector(0, 1, 0);
					radiansOfMovement = Math.PI;
					break;
				case HardPointPosition.Rear:
					origin = new Vector(0, -1, 0);
					radiansOfMovement = Math.PI;
					break;
				case HardPointPosition.Left:
					origin = new Vector(-1, 0, 0);
					radiansOfMovement = Math.PI / 2;
					break;
				case HardPointPosition.Right:
					origin = new Vector(1, 0, 0);
					radiansOfMovement = Math.PI / 2;
					break;
				case HardPointPosition.Top:
					origin = new Vector(0, 1, 0);
					radiansOfMovement = Math.PI * 2;
					break;
				case HardPointPosition.Bottom:
					origin = new Vector(0, 1, 0);
					radiansOfMovement = Math.PI * 2;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private double _speed;

		/// <summary>Speed of this hard point, measured from 0..1.  0 means the hard point is fixed and cannot move, 1 means the hard point can travel its full rotational degree in one pass</summary>
		public double Speed
		{
			get { return _speed; }
			set
			{
				Ensure.That(value, "value").IsInRange(0d, 1d);
				_speed = value;
			}
		}

		public HardPointPosition Position { get; private set; }
		public Ship Ship { get; private set; }
		public Weapon Weapon { get; private set; }
		public double AttackCounter { get; set; }

		/// <summary>Current position (in Degrees)</summary>
		public double Inclination
		{
			get
			{
				var radians = CalculateCurrentAngle(Orientation);
				var degrees = radians*(0.5/Math.PI)*360;
				return Math.Abs(Math.Round(degrees, 4));
			}
		}

		/// <summary>Current position (as a UnitVector)</summary>
		public Vector Orientation
		{
			get { return _orientation; }
			private set
			{
				if ( value == Vector.Zero )
				{
					_orientation = _origin;
				}
				else
				{
					value = value.ToUnitVector();
					var newAngle = CalculateCurrentAngle(value);
					if (newAngle >= _radiansOfMovement)
					{
						throw new ArgumentOutOfRangeException("value", "Cannot move to new position because it is out of range");
					}

					_orientation = value;
				}
			}
		}

		public Vector Origin
		{
			get { return _origin; }
		}

		private double CalculateCurrentAngle(Vector orientation)
		{
			Vector rightExtent = _origin.RotateZ(_radiansOfMovement / -2d);
			return Vector.Angle(rightExtent, orientation) - (_radiansOfMovement / 2d);
		}

		public static HardPoint FactoryHardPoint(HardPointPosition position)
		{
			return new HardPoint(position);
		}

		public void EquipWeapon(Weapon weapon)
		{
			Weapon = weapon;
			Weapon.HardPoint = this;
			Weapon.Position = new Position(Ship, Vector.Zero);
		}

		public bool AimAt(ILocation target)
		{
			var orientation = GetAbsoluteOrientation(Orientation);

			Vector targetPosition = (target.Position.UniversalCoordinates - Ship.Position.UniversalCoordinates);
			targetPosition = targetPosition.ToUnitVector();
			
			double extent = Vector.Angle(_origin, targetPosition);
			double angleToMove = Vector.Angle(orientation, targetPosition);

			if ( extent - (_radiansOfMovement / 2) > Units.Tolerance )
			{
				angleToMove = Math.Min(_radiansOfMovement * Speed, _radiansOfMovement / 2);
				Vector counter = _origin.RotateZ(angleToMove);
				Vector clock = _origin.RotateZ(angleToMove*-1d);

				Orientation = (counter - targetPosition).Magnitude < (clock - targetPosition).Magnitude ? counter : clock;
				return false;
			}

			if ( angleToMove - (_radiansOfMovement * Speed) > Units.Tolerance )
			{
				angleToMove = _radiansOfMovement * Speed;
				Vector counter = orientation.RotateZ(angleToMove);
				Vector clock = orientation.RotateZ(angleToMove*-1d);

				Orientation = (counter - targetPosition).Magnitude < (clock - targetPosition).Magnitude ? counter : clock;
				return false;
			}

			Orientation = targetPosition;
			return true;
		}

		/// <summary>
		/// Returns true if a hard point can aim at a particular location
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public bool InRange(ILocation target)
		{
			if (target.Position.UniversalCoordinates == Ship.Position.UniversalCoordinates)
			{
				throw new ArgumentException("Target must not be at the same position as the ship");
			}

			Vector targetPosition = (target.Position.UniversalCoordinates - Ship.Position.UniversalCoordinates);
			if (targetPosition == Vector.Zero)
				throw new ArgumentException("Target is co-incident", "target");

			targetPosition = targetPosition.ToUnitVector();

			var orientation = GetAbsoluteOrientation(Origin);

			double extent = Vector.Angle(orientation, targetPosition);
			return (extent - (_radiansOfMovement / 2) < Units.Tolerance);
		}

		/// <summary>
		/// Returns true if a hard point can move in time to aim at a target
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public bool CanTrack(ILocation target)
		{
			Vector targetPosition = (target.Position.UniversalCoordinates - Ship.Position.UniversalCoordinates);
			if (targetPosition == Vector.Zero)
				throw new ArgumentException("Target is co-incident", "target");

			targetPosition = targetPosition.ToUnitVector();

			var orientation = GetAbsoluteOrientation(Origin);
			double extent = Math.Acos(orientation.DotProduct(targetPosition));

			if ( extent - (_radiansOfMovement / 2) > Units.Tolerance )
				return false;

			orientation = GetAbsoluteOrientation(Orientation);
			double angleToMove = Vector.Angle(orientation, targetPosition);
			if ( angleToMove - (_radiansOfMovement * Speed) > Units.Tolerance )
				return false;

			return true;
		}

		public void Reset()
		{
			Orientation = _origin;
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