using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Statistics;

namespace Echo
{
	public class Orbiter
	{
		private readonly Universe _universe;
		private const double GravitationalConstant = 6.673E-11;

		public Orbiter(Universe universe, List<TickRegistration> tickRegistrations)
		{
			_universe = universe;
			tickRegistrations.Add(new TickRegistration(Orbit));
		}

		public long Orbit(TickContext context)
		{
			foreach (var solarSystem in _universe.StarClusters.SelectMany(s => s.SolarSystems))
			{
				foreach (var celestialBody in solarSystem.Satellites.Where(x => x.Position.LocalCoordinates != Vector.Zero))
					Move(celestialBody, Speed(celestialBody) * context.ElapsedTicks);

				foreach (var structure in solarSystem.Structures.Where(x => x.Position.LocalCoordinates != Vector.Zero))
					Move(structure, context.ElapsedTicks);
			}

			return 1;			
		}

		private double Speed(CelestialObject celestialBody)
		{
			var u = celestialBody.Mass * GravitationalConstant;
			var a = celestialBody.Position.LocalCoordinates.Magnitude;
			return System.Math.Sqrt(u / a);
		}

		private void Move(IMoves target, double distanceTravelled)
		{
			var orbitRadius = target.Position.LocalCoordinates.Magnitude;
			var radiansTravelled = (distanceTravelled / orbitRadius);

			target.Position = target.Position.RotateZ(radiansTravelled);
		}
	}
}