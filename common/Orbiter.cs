using System;
using Echo.Celestial;

namespace Echo
{
	public class Orbiter
	{
		private const double GravitationalConstant = 6.673E-27;
		private readonly SolarSystem _solarSystem;

		public Orbiter(SolarSystem solarSystem)
		{
			_solarSystem = solarSystem;
		}

		public TickRegistration TickRegistration => new TickRegistration(Orbit);

		public long Orbit(TickContext context)
		{
			foreach (var celestialBody in _solarSystem.Satellites)
				Move(celestialBody, Speed(celestialBody) * context.ElapsedTicks);

			foreach (var structure in _solarSystem.Structures)
				Move(structure, context.ElapsedTicks);

			return 1;
		}

		private double Speed(CelestialObject celestialBody) => Math.PI / 1000;

		private void Move(IMoves target, double distanceTravelled)
		{
			var orbitRadius = target.Position.LocalCoordinates.Magnitude;
			var radiansTravelled = (distanceTravelled / orbitRadius);

			target.Position = target.Position.RotateZ(radiansTravelled);
		}
	}
}