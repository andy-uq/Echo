using System;
using System.Diagnostics;

namespace Echo.Vectors
{
	[DebuggerDisplay("X={X}, Y={Y}, Z={Z} [{Magnitude}]")]
	public struct Vector : IEquatable<Vector>, IFormattable
	{
		public readonly static Vector Zero = new Vector(0, 0, 0);

		public Vector(double x, double y, double z) : this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public double X { get; private set; }
		public double Y { get; private set; }
		public double Z { get; private set; }

		public double Magnitude
		{
			get
			{
				double raw = Math.Sqrt(X*X + Y*Y + Z*Z);
				return Math.Round(raw, 4);
			}
		}

		#region IEquatable<Vector> Members

		public bool Equals(Vector obj)
		{
			return AreWithinTolerance(obj.X, X) && AreWithinTolerance(obj.Y, Y) && AreWithinTolerance(obj.Z, Z);
		}

		private static bool AreWithinTolerance(double a, double b)
		{
			return Math.Round(a, 4) == Math.Round(b, 4);
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof (Vector))
				return false;
			return Equals((Vector) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = X.GetHashCode();
				result = (result*397) ^ Y.GetHashCode();
				result = (result*397) ^ Z.GetHashCode();
				return result;
			}
		}

		public string ToString(string format, IFormatProvider fp)
		{
			if ( String.IsNullOrEmpty(format) )
				format = "G";

            // If G format specifier is passed, display like this: (x, y).
			if ( format.ToLower() == "g" )
				return String.Format("({0:n4}, {1:n4})", X, Y);

			// For "x" formatting, return just the x value as a string
			if ( format.ToLower() == "x" )
				return X.ToString("n4");

			// For "y" formatting, return just the y value as a string
			if ( format.ToLower() == "y" )
				return Y.ToString("n4");

			// For "l" formatting, return just the magnitude as a string
			if ( format.ToLower() == "l" )
				return Magnitude.ToString("n4");

			// For any unrecognized format, throw an exception.
			throw new FormatException(String.Format("Invalid format string: '{0}'.", format));
		}

		public static bool operator ==(Vector left, Vector right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector left, Vector right)
		{
			return !left.Equals(right);
		}

		public Vector ToUnitVector()
		{
			double magnitude = Magnitude;

			if (magnitude == 0d)
				throw new ArgumentException("Cannot create a unit vector from the Zero vector");

			if (magnitude == 1d)
				return new Vector(X, Y, Z);

			var r = new Vector();
			r.X = X/Magnitude;
			r.Y = Y/magnitude;
			r.Z = Z/magnitude;

			return r;
		}

		public Vector Scale(double scale)
		{
			var r = new Vector();

			r.X = X*scale;
			r.Y = Y*scale;
			r.Z = Z*scale;

			return r;
		}

		public double DotProduct(Vector b)
		{
			return DotProduct(this, b);
		}

		public static double DotProduct(Vector a, Vector b)
		{
			return (a.X*b.X) + (a.Y*b.Y) + (a.Z*b.Z);
		}

		public static double Angle(Vector a, Vector b)
		{
			a = a.ToUnitVector();
			b = b.ToUnitVector();

			var dotProduct = Math.Round(DotProduct(a, b), 4);
			return Math.Acos(dotProduct);
		}

		public Vector RotateZ(double radians)
		{
			var r = new Vector();

			r.X = Math.Cos(radians)*X - Math.Sin(radians)*Y;
			r.Y = Math.Sin(radians)*X + Math.Cos(radians)*Y;

			return r;
		}

		public static Vector operator +(Vector a, Vector b)
		{
			var r = new Vector();

			r.X = a.X + b.X;
			r.Y = a.Y + b.Y;
			r.Z = a.Z + b.Z;

			return r;
		}

		public static Vector operator -(Vector a, Vector b)
		{
			var r = new Vector();

			r.X = a.X - b.X;
			r.Y = a.Y - b.Y;
			r.Z = a.Z - b.Z;

			return r;
		}

		public static Vector operator *(Vector a, Vector b)
		{
			var r = new Vector();

			r.X = (a.Y*b.Z) - (b.Y*a.Z);
			r.Y = (b.X*a.Z) - (b.Z*a.X);
			r.Z = (a.X*b.Y) - (b.X*a.Y);

			return r;
		}

		public static double Distance(Vector a, Vector b)
		{
			return (a - b).Magnitude;
		}

		public static bool Intersects(Vector p0, double r0, Vector p1, double r1)
		{
			/*
				a = (r02 - r12 + d2 ) / (2 d)
				h2 = r02 - a2
				P2 = P0 + a ( P1 - P0 ) / d
				x3 = x2 +- h ( y1 - y0 ) / d
				y3 = y2 -+ h ( x1 - x0 ) / d
			*/

			var tmp = (p1 - p0);
			double d = tmp.Magnitude;

			if (d > (r0 + r1))
				return false;

			if (d < Math.Abs(r0 - r1))
				return true;

			if (d == 0 && r0 == r1)
				return true;

			return true;
		}
	}
}