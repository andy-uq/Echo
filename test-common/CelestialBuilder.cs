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
	}

	public class SolarSystemBuilder : CelestialBuilder
	{
		private readonly SolarSystemState _solarSystem;
		private List<CelestialObjectState> _satellites;

		public SolarSystemBuilder()
		{
			_solarSystem = new SolarSystemState();
		}

		public SolarSystemBuilder Planet(Vector localCoordinates = default(Vector))
		{
			var planet = new CelestialObjectState
			{
				LocalCoordinates = localCoordinates,
			};

			_satellites.Add(planet);
			return this;
		}

		public SolarSystemState Build()
		{
			return new SolarSystemState
			{
				Satellites = new System.Collections.Generic.,
			};
		}
	}
}
