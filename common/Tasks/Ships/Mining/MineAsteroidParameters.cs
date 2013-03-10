using Echo.Celestial;
using Echo.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public class MineAsteroidParameters : ITaskParameters
	{
		private readonly Ship _ship;
		private readonly AsteroidBelt _asteroidBelt;

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