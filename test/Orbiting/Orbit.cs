using System;
using System.Collections.Generic;
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
			var context = new TickContext(0) { ElapsedTicks = 60*60*5 };
			var universe = Universe.Builder.Build(world.Universe).Materialise();
			var orbiter = new Orbiter(universe, new List<TickRegistration>());

			var earth = universe.StarClusters
				.SelectMany(x => x.SolarSystems)
				.SelectMany(x => x.Satellites)
				.Single(x => x.Id == world.Earth.ObjectId);

			Console.WriteLine(earth.Position.LocalCoordinates);
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