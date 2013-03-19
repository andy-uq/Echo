using System;
using Echo;
using Echo.Items;

namespace Echo.Celestial
{
	public class AsteroidBelt : CelestialObject
	{
		public override CelestialObjectType CelestialObjectType
		{
			get { return CelestialObjectType.AsteriodBelt; }
		}

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