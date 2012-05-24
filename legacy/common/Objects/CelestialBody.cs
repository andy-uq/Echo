using System;
using System.Collections.Generic;

using Echo.Maths;
using Echo.Vectors;

namespace Echo.Objects
{
	public abstract class CelestialBody : OrbitingObject
	{
		private readonly List<OrbitingObject> satellites = new List<OrbitingObject>();

		public List<OrbitingObject> Satellites
		{
			get { return this.satellites; }
		}

		public double Size { get; set; }

		protected CelestialBody(SolarSystem solarSystem) 
			: base(solarSystem)
		{
		}

		protected CelestialBody(CelestialBody orbiting)
			: base(orbiting)
		{
		}

		public void AddSatellite(OrbitingObject satellite, double timeToOrbit)
		{
			var r = timeToOrbit / (Math.PI * 2d);
			var radians = Rand.Next() * Math.PI * 2d;
			var delta = new Vector(Math.Cos(radians) * r, Math.Sin(radians) * r, 0);

			AddSatellite(satellite, delta);
		}

		public void AddSatellite(OrbitingObject satellite, Vector delta)
		{
			satellite.Location = this;
			satellite.LocalCoordinates = delta;
			this.satellites.Add(satellite);

			Extent = Max(Extent, satellite.LocalCoordinates.Magnitude + satellite.Extent, Size);
		}

		private static T Max<T>(params T[] values) where T : IComparable<T>
		{
			if (values.Length == 0)
				throw new InvalidOperationException("Max function requires at least one operand");

			T max = values[0];

			for (int i = 1; i < values.Length; i++)
			{
				T value = values[i];
				if (value.CompareTo(max) > 0)
					max = value;
			}

			return max;
		}

		/// <summary>The mass of this celestial body</summary>
		public double Mass { get; protected set; }

		/// <summary>Calculates the initial position for an object that wants to orbit this celestial body</summary>
		/// <param name="timeToOrbit">Time to complete one revolution</param>
		public Vector FromTimeToOrbit(double timeToOrbit)
		{
			return new Vector(timeToOrbit / (Math.PI * 2d), 0, 0);
		}

		public override void Tick(ulong tick)
		{
			base.Tick(tick);
			this.satellites.ForEach(s => s.Tick(tick));
		}
	}
}