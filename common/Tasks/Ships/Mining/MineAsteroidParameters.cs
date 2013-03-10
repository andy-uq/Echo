using Echo.Celestial;
using Echo.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public class MineAsteroidParameters : ITaskParameters
	{
		private readonly AsteroidBelt _asteroidBelt;
		private readonly Ship _ship;

		public MineAsteroidParameters(Ship ship, AsteroidBelt asteroidBelt)
		{
			_ship = ship;
			_asteroidBelt = asteroidBelt;
		}

		public Ship Ship
		{
			get { return _ship; }
		}

		public AsteroidBelt AsteroidBelt
		{
			get { return _asteroidBelt; }
		}
	}
}