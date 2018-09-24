using System;
using System.Collections.Generic;
using Echo;
using Echo.State;

namespace test.common
{
	public class CelestialBuilder
	{
		public SolarSystemBuilder SolarSystem()
		{
			return new SolarSystemBuilder();
		}

		public SolarSystemState SolarSystem(Action<SolarSystemBuilder> builder)
		{
			var solarSystem = SolarSystem();
			builder(solarSystem);

			return solarSystem.Build();
		}

		public StarClusterState StarCluster(Action<StarClusterBuilder> builder)
		{
			var starCluster = new StarClusterBuilder();
			builder(starCluster);

			return starCluster.Build();
		}
	}

	public class StarClusterBuilder
	{
		private readonly StarClusterState _starCluster;
		private readonly List<SolarSystemState> _solarSystems;

		public StarClusterBuilder()
		{
			_starCluster = new StarClusterState();
			_solarSystems = new List<SolarSystemState>();
		}

		public StarClusterBuilder SolarSystem(Vector localCoordinates = default)
		{
			var solarSystem = new SolarSystemState
			{
				LocalCoordinates = localCoordinates
			};

			_solarSystems.Add(solarSystem);

			return this;
		}

		public StarClusterState Build()
		{
			return new StarClusterState
			{
				SolarSystems = _solarSystems.ToArray()
			};
		}
	}

	public class SolarSystemBuilder : CelestialBuilder
	{
		private readonly SolarSystemState _solarSystem;
		private readonly List<CelestialObjectState> _satellites;

		public SolarSystemBuilder()
		{
			_solarSystem = new SolarSystemState();
			_satellites = new List<CelestialObjectState>();
		}

		public SolarSystemBuilder Planet(Vector localCoordinates = default)
		{
			var planet = new CelestialObjectState
			{
				LocalCoordinates = localCoordinates
			};

			_satellites.Add(planet);
			return this;
		}

		public SolarSystemState Build()
		{
			return new SolarSystemState
			{
				Satellites = _satellites.ToArray()
			};
		}
	}
}
