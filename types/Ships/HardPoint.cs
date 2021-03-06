﻿using System;
using System.Diagnostics;

namespace Echo.Ships
{
	[DebuggerDisplay("{Position}, Equipped: {Weapon}")]
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
			CalculateHardPoint(position, out var origin, out _);
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
			get => _speed;
			set
			{
				if (value < 0d || value > 1d)
					throw new ArgumentOutOfRangeException(nameof(value), value, "Speed must be 0/0 <= speed <= 1.0");

				_speed = value;
			}
		}

		public HardPointPosition Position { get; }
		public Ship Ship { get; }
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
			get => _orientation;
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
						throw new ArgumentOutOfRangeException(nameof(value), "Cannot move to new position because it is out of range");
					}

					_orientation = value;
				}
			}
		}

		public Vector Origin => _origin;

		private double CalculateCurrentAngle(Vector orientation)
		{
			var rightExtent = _origin.RotateZ(_radiansOfMovement / -2d);
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
			var targetPosition = (target.Position - Ship.Position);
			return AimAt(targetPosition.ToUnitVector());
		}

		public bool AimAt(Vector targetPosition)
		{
			targetPosition = CalculateTargetPosition(targetPosition);

			var orientation = GetAbsoluteOrientation(Orientation);
			var angleToMove = Vector.Angle(orientation, targetPosition);
			if (Math.Abs(angleToMove) < Units.Tolerance)
			{
				return false;
			}

			if ( angleToMove - (_radiansOfMovement * Speed) > Units.Tolerance )
			{
				angleToMove = _radiansOfMovement * Speed;
				var counter = orientation.RotateZ(angleToMove);
				var clock = orientation.RotateZ(-angleToMove);

				Orientation = (counter - targetPosition).Magnitude < (clock - targetPosition).Magnitude ? counter : clock;
				return false;
			}

			Orientation = targetPosition;
			return true;
		}

		private Vector CalculateTargetPosition(Vector targetPosition)
		{
			var extent = Vector.Angle(_origin, targetPosition);
			var maxExtent = _radiansOfMovement/2;
			if (extent - maxExtent <= Units.Tolerance)
				return targetPosition;

			var counter = _origin.RotateZ(maxExtent);
			var clock = _origin.RotateZ(-maxExtent);

			return (counter - targetPosition).Magnitude < (clock - targetPosition).Magnitude
				? counter
				: clock;
		}

		/// <summary>
		/// Returns true if a hard point can aim at a particular location
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public bool InRange(ILocation target)
		{
			var targetPosition = (target.Position.UniversalCoordinates - Ship.Position.UniversalCoordinates);
			if (targetPosition == Vector.Zero)
				throw new ArgumentException("Target must not be at the same position as the ship");
			
			targetPosition = targetPosition.ToUnitVector();

			var orientation = GetAbsoluteOrientation(Origin);

			var extent = Vector.Angle(orientation, targetPosition);
			return (extent - (_radiansOfMovement / 2) < Units.Tolerance);
		}

		/// <summary>
		/// Returns true if a hard point can move in time to aim at a target
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public bool CanTrack(ILocation target)
		{
			var targetPosition = (target.Position.UniversalCoordinates - Ship.Position.UniversalCoordinates);
			if (targetPosition == Vector.Zero)
				throw new ArgumentException("Target must not be at the same position as the ship");

			targetPosition = targetPosition.ToUnitVector();

			var orientation = GetAbsoluteOrientation(Origin);
			var extent = Math.Acos(orientation.DotProduct(targetPosition));

			if ( extent - (_radiansOfMovement / 2) > Units.Tolerance )
				return false;

			orientation = GetAbsoluteOrientation(Orientation);
			var angleToMove = Vector.Angle(orientation, targetPosition);
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