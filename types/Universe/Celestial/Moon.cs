using System;
using Echo.Items;

namespace Echo.Celestial
{
	public class AsteroidBelt : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType => CelestialObjectType.AsteroidBelt;

		public ItemCode Ore { get; set; }
		public int Richness { get; set; }
		public uint AmountRemaining { get; set; }
		public uint Difficulty { get; set; }

		public uint Reduce(uint quantity)
		{
			var remaining = Math.Min(AmountRemaining, quantity);
			AmountRemaining -= remaining;

			return remaining;
		}
	}

	public class Moon : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType => CelestialObjectType.Moon;

		public Planet Planet => (Planet) Position.Location;
	}

	public class Planet : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType => CelestialObjectType.Planet;

		public SolarSystem Sun => (SolarSystem) Position.Location;
	}
}