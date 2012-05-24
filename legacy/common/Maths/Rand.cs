using System;

namespace Echo.Maths
{
	public static class Rand
	{
		private static Random rand;

		public static void Initialise(int seed)
		{
			if ( seed == 0 )
			{
				rand = new Random();
			}
			else
			{
				rand = new Random(seed);
			}
		}

		public static uint Next(uint minValue, uint maxValue)
		{
			if (rand == null)
				Initialise(0);

			return (uint )(rand.NextDouble() * (maxValue - minValue) + minValue);
		}

		public static double Next()
		{
			if ( rand == null )
				Initialise(0);

			return rand.NextDouble();
		}
	}
}