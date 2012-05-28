using System.Collections.Generic;
using Echo.State;
using Echo;

namespace Echo.Tests
{
	public class MockUniverse
	{
		private long _nextId;

		public StructureState Manufactory { get; set; }
		public CelestialObjectState Moon { get; set; }
		public CelestialObjectState Earth { get; set; }
		public CelestialObjectState AsteroidBelt { get; set; }
		public SolarSystemState SolarSystem { get; set; }
		public StarClusterState StarCluster { get; set; }
		public UniverseState Universe { get; set; }

		public MockUniverse()
		{
			Earth = new CelestialObjectState
			{
				Id = Id(),
				CelestialObjectType = CelestialObjectType.Planet,
				Name = "Earth",
				Mass = 50d,
				Size = 5d,
			};
			AsteroidBelt = new CelestialObjectState()
			{
				Id = Id(),
				CelestialObjectType = CelestialObjectType.AsteriodBelt,
				Name = "Asteroid Belt",
				OrbitsId = 1,
				Mass = 0d,
				LocalCoordinates = new Vector(5.1, 0, 0),
				AsteroidBelt = new AsteroidBeltState
				{
					Richness = 500000,
					AmountRemaining = 0,
				},
			};
			Moon = new CelestialObjectState
			{
				Id = Id(),
				CelestialObjectType = CelestialObjectType.Moon,
				Name = "Moon",
				OrbitsId = Earth.Id,
				Mass = 0.5d,
				Size = 0.5d,
				LocalCoordinates = new Vector(7.5, 0, 0)
			};
			Manufactory = new StructureState()
			{
				Id = Id(),
				Name = "MFC",
				OrbitsId = Moon.Id,
				LocalCoordinates = new Vector(0.5001, 0, 0),
				Manufactory = new ManufactoryState() { Efficiency = 0.5d },
			};
			SolarSystem = new SolarSystemState
			{
				Id = Id(),
				Name = "Sol",
				Satellites = new[] { Earth, Moon, AsteroidBelt, },
				Structures = new[] { Manufactory }
			};
			StarCluster = new StarClusterState
			{
				Id = Id(),
				Name = "Revvon",
				SolarSystems = new[] { SolarSystem },
			};
			Universe = new UniverseState
			{
				Id = Id(),
				StarClusters = new List<StarClusterState>() {StarCluster}
			};
		}

		private long Id()
		{
			return ++_nextId;
		}
	}
}