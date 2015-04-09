using System;

namespace Echo
{
	public static class Units
	{
		public const double EarthRadius = 6378.1;
		public const double MoonRadius = 1738.1;
		public const double Tolerance = 0.0001;
		public const double Epsilon = 0.00001;

		/// <summary>150,000,000 km per AU</summary>
		public const double AstronomicalUnits = 150;

		public static double ToAU(double d)
		{
			return d / AstronomicalUnits;
		}

		public static double FromAU(double d)
		{
			return d * AstronomicalUnits;
		}

		public static bool IsZero(double value)
		{
			return Math.Abs(value) < Epsilon;
		}

		public static bool Equal(double a, double b)
		{
			return Math.Abs(a - b) < Epsilon;
		}
	}
}