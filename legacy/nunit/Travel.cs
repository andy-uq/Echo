using System;

using Echo.Entities;
using Echo.Maths;
using Echo.Objects;
using Echo.Ships;
using Echo.Vectors;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class TravelTests
	{
		[Test]
		public void IntraSolarSystem()
		{
			var universe = new Universe();
			var sol = new SolarSystem { Location = universe };

			var ship = new Ship(new Agent(new Corporation())) {Speed = 2d, Name = "S1" };
			sol.EnterSystem(ship, new Vector(10, 0, 0));

			ship.Destination = Vector.Zero;

			for (int i=10; i >= 0; i-=2)
			{
				Assert.AreEqual(i, ship.DistanceToDestination, "Ship is heading in the wrong direction!");
				ship.Tick((uint )i);
			}
		}

		[Test]
		public void JumpGates()
		{
			var universe = new Universe();
			
			var sol = new SolarSystem { Name = "Sol", Location = universe };
			var outOfSol = new JumpGate(sol);
			sol.OrbitSun(outOfSol, 45d);

			var alphaCentauri = new SolarSystem {Name = "Alpha Centauri", UniversalCoordinates = new Vector(1000d, 0, 0), Location = universe };

			var intoAlphaCentauri = new JumpGate(alphaCentauri);
			alphaCentauri.OrbitSun(intoAlphaCentauri, 45d);

			outOfSol.ConnectsTo = intoAlphaCentauri;

			var ship = new Ship(new Agent(new Corporation())) {Speed = 2d };
			sol.EnterSystem(ship, outOfSol.LocalCoordinates);

			Assert.AreEqual(sol, ship.SolarSystem);
			Assert.AreEqual(outOfSol.UniversalCoordinates, ship.UniversalCoordinates);

			CollectionAssert.Contains(sol.Objects, ship);
			CollectionAssert.DoesNotContain(alphaCentauri.Objects, ship);
			
			outOfSol.Jump(ship);

			Assert.AreEqual(alphaCentauri, ship.SolarSystem);
			CollectionAssert.DoesNotContain(sol.Objects, ship);
			CollectionAssert.Contains(alphaCentauri.Objects, ship);
	
			Assert.AreEqual(intoAlphaCentauri.UniversalCoordinates, ship.UniversalCoordinates);
		}

		[Test]
		public void MoveToJumpGate()
		{
			var universe = new Universe();
			var starCluster = new StarCluster();
			universe.AddStarCluster(starCluster);

			var sol = new SolarSystem { Name = "Sol" };
			starCluster.AddSolarSystem(sol);

			var alphaCentauri = new SolarSystem { Name = "Alpha Centauri", UniversalCoordinates = new Vector(1.6E9d, 0, 0)};
			starCluster.AddSolarSystem(alphaCentauri);

			var outOfSol = new JumpGate(sol);
			sol.OrbitSun(outOfSol, 45d);

			var intoAlphaCentauri = new JumpGate(alphaCentauri);
			alphaCentauri.OrbitSun(intoAlphaCentauri, 45d);

			outOfSol.ConnectsTo = intoAlphaCentauri;

			var ship = new Ship(new Agent(new Corporation())) { Speed = 20d };
			sol.EnterSystem(ship, new Vector(100d, 0, 0));

			ship.SetTask(new ShipTask(outOfSol, delegate { outOfSol.Jump(ship); return true; }));

			ulong tick = 0L;
			while (ship.SolarSystem == sol)
			{
				Assert.Less(tick, 1000L, "Ship took more than 1000 days to reach the jump gate.");

				starCluster.Tick(tick);
				tick++;
			}

			Console.WriteLine("Ship took {0} days to reach its destination which was a distance of {1:n0}km", tick, (intoAlphaCentauri.UniversalCoordinates - sol.UniversalCoordinates).Magnitude);
		}

		[Test]
		public void RefineOre()
		{
			Rand.Initialise(0);

			var universe = new Universe();
			var sol = new SolarSystem { Location = universe };

			var corporation = new Corporation() { Location = universe };
			var mPilot = corporation.Recruit();
			var tPilot = corporation.Recruit();

			var asteroidBelt = new AsteroidBelt(sol) {Richness = 100};
			sol.OrbitSun(asteroidBelt, 100d);
            
			var refinery = new Refinery(sol, corporation) {OreProcessedPerTick = 5, Efficiency = 2.5d };
			sol.OrbitSun(refinery, 200d);

			var manufactory = new Manufactory(sol, corporation);
			sol.OrbitSun(manufactory, 400d);

			var m1 = new Ship(mPilot) { Speed = 5d, CargoHoldSize = 100d, Name = "M1", UniversalCoordinates = refinery.UniversalCoordinates };
			refinery.Dock(m1);

			var t1 = new Ship(tPilot) { Speed = 5d, CargoHoldSize = 10d, Name = "T1", UniversalCoordinates = refinery.UniversalCoordinates };
			refinery.Dock(t1);

			var start = new ShipTask(asteroidBelt, (ship, target) => asteroidBelt.Mine(ship));
			start = start.Join(refinery, (ship, target) => refinery.UnloadOre(ship)).Join(start);

			refinery.Undock(m1, mPilot);
			m1.SetTask(start);

			start = new ShipTask(refinery, (ship, target) => { refinery.LoadRefinedOre(ship); return ship.Cargo.Count > 0 && ship.Cargo[0].Quantity > 50; });
			start = start.Join(manufactory, (ship, target) => manufactory.UnloadRefinedOre(ship)).Join(start);

			refinery.Undock(t1, tPilot);
			t1.SetTask(start);

			CollectionAssert.Contains(sol.Objects, m1);
			CollectionAssert.Contains(sol.Objects, t1);

			ulong tick = 0L;
			while ( manufactory.OreRemaining < 100 )
			{
				Assert.Less(tick, 1000, "Ssytem took more than 1000 days to complete the task.  {0:n0} products manufactured", manufactory.ProductCount);

				sol.Tick(tick);
				tick++;
			}

			Console.WriteLine("System took {0} days to manufacture {1:n0} items", tick, manufactory.ProductCount);
			Console.WriteLine("The refinery had {0:n0} unprocessed ore", refinery.UnrefinedOre);
			Console.WriteLine("The manufactory has {0:n0} unprocessed ore for building", manufactory.OreRemaining);
		}
	}
}