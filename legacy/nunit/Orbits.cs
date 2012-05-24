using System;

using Echo.Maths;
using Echo.Objects;
using Echo.Vectors;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class Orbits
	{
		[Test]
		public void Orbiting()
		{
			Rand.Initialise(0);
			var sol = new SolarSystem();

			var earth = new Planet(sol);
			sol.OrbitSun(earth, 365d);

			var moon = new Moon(earth);
			earth.AddSatellite(moon, 28d);

			double earthOrbitRadius = (earth.UniversalCoordinates - sol.UniversalCoordinates).Magnitude;
			double moonOrbitRadius = (moon.UniversalCoordinates - earth.UniversalCoordinates).Magnitude;

            for (uint i=0; i <= 365; i++)
			{
				sol.Tick(i);

				Assert.AreEqual(earthOrbitRadius, (earth.UniversalCoordinates - sol.UniversalCoordinates).Magnitude, "Earth hit escape velocity!");
				Assert.AreEqual(moonOrbitRadius, (moon.UniversalCoordinates - earth.UniversalCoordinates).Magnitude, "Moon hit escape velocity!");
				
				Console.WriteLine("{0:n4},{1:n4},{2:n4},{3:n4}", earth.UniversalCoordinates.X, earth.UniversalCoordinates.Y, moon.UniversalCoordinates.X, moon.UniversalCoordinates.Y);
			}
		}
	}
}