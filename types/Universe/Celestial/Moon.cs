﻿using Echo;

namespace Echo.Celestial
{
	public class AsteroidBelt : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType
		{
			get { return CelestialObjectType.AsteriodBelt; }
		}

		public uint Richness { get; set; }
		public uint AmountRemaining { get; set; }
	}

	public class Moon : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType
		{
			get { return CelestialObjectType.Moon; }
		}

		public Planet Planet
		{
			get { return (Planet) Position.Location; }
		}
	}

	public class Planet : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType
		{
			get { return CelestialObjectType.Planet; }
		}

		public SolarSystem Sun
		{
			get { return (SolarSystem) Position.Location; }
		}
	}
}