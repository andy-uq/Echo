using System;
using System.Linq;
using Echo.Tests.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Orbiting
{
	public class Orbit
	{
		[Test]
		public void EarthOrbit()
		{
			var world = new MockUniverse();
			var universe = Universe.Builder.Build(world.Universe).Materialise();

			var sol = universe.StarClusters
				.SelectMany(x => x.SolarSystems)
				.Single(x => x.Id == world.SolarSystem.ObjectId);

			var earth = sol.Satellites
				.Single(x => x.Id == world.Earth.ObjectId);

			var context = new TickContext(0) { ElapsedTicks = 60 * 60 * 5 };
			var orbiter = new Orbiter(sol);
			var orbitRadius = earth.Position.LocalCoordinates.Magnitude;

			for (var i = 0; i < 12*21; i++)
			{
				orbiter.Orbit(context);
				Console.WriteLine("{0}: {1:n}", i, earth.Position.LocalCoordinates * 1e-5);
				
				earth.Position.LocalCoordinates.Magnitude.ShouldBe(orbitRadius, Units.Tolerance*100);
			}
		}
	}
}