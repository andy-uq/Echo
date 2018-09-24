using Echo.Celestial;
using Echo.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public class MineAsteroidParameters : ITaskParameters
	{
		public MineAsteroidParameters(Ship ship, AsteroidBelt asteroidBelt)
		{
			Ship = ship;
			AsteroidBelt = asteroidBelt;
		}

		public Ship Ship { get; }
		public AsteroidBelt AsteroidBelt { get; }
	}
}