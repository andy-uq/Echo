using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Engine;
using Echo.State;
using Echo.Tests.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.TheGame
{
	public class GameUpdate
	{
		[Test]
		public void Update()
		{
			var world = new MockUniverse();
			var universe = Universe.Builder.Build(world.Universe).Materialise();

			var sol = universe.StarClusters
				.SelectMany(x => x.SolarSystems)
				.Single(x => x.Id == world.SolarSystem.ObjectId);

			var earth = sol.Satellites
				.Single(x => x.Id == world.Earth.ObjectId);

			var game = new Game(universe, u => Orbit(Satellites(u)));

			var ticksRemaining = game.Update();
			ticksRemaining.ShouldBeGreaterThan(0);

			earth.Position.LocalCoordinates.ShouldNotBe(world.Earth.LocalCoordinates);
		}

		private IEnumerable<SolarSystem> Satellites(Universe universe)
		{
			return universe.StarClusters
				.SelectMany(s => s.SolarSystems);
		}

		private IEnumerable<TickRegistration> Orbit(IEnumerable<SolarSystem> solarSystems)
		{
			return solarSystems
				.Select(s => new Orbiter(s))
				.Select(o => o.TickRegistration);
		}
	}
}