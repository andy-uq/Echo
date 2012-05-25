namespace Echo.Maths
{
	public static class Units
	{
		public const double EarthRadius = 6378.1;
		public const double MoonRadius = 1738.1;
		public const double Tolerance = 0.0001;

		/// <summary>150,000,000 km per AU</summary>
		public const double AstronomicalUnits = 1.5E8;

		public static double ToAU(double d)
		{
			return d/AstronomicalUnits;
		}

		public static double FromAU(double d)
		{
			return d*AstronomicalUnits;
		}
	}
}