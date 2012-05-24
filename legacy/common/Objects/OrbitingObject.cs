using System;

using Echo.Vectors;

namespace Echo.Objects
{
	/// <summary>A universe object that orbits another object around the Z axis</summary>
	public abstract class OrbitingObject : BaseLocation
	{
		private const double PIx2 = (2d*Math.PI);

		private double orbitDisplacement;
		private double orbitRadius;

		/// <summary>Create a new universe object orbiting another object</summary>
		/// <param name="orbiting">Object to orbit</param>
		protected OrbitingObject(ILocation orbiting)
		{
			if (orbiting == null)
				throw new ArgumentNullException("orbiting");

			Location = orbiting;
			Orbiting = orbiting;
			Speed = 1d;
		}

		/// <summary>Speed at which this body orbits</summary>
		public double Speed { get; set; }

		public ILocation Orbiting { get; private set; }

		public override ObjectType ObjectType
		{
			get { return ObjectType.OrbitingBody; }
		}

		public override Vector UniversalCoordinates
		{
			get { return base.UniversalCoordinates; }
			set
			{
				base.UniversalCoordinates = value;
				CalculateOrbitFactors();
			}
		}

		private void CalculateOrbitFactors()
		{
			if (Orbiting == null)
				return;

			// calculate our orbit radius
			this.orbitRadius = (this.UniversalCoordinates - Orbiting.UniversalCoordinates).Magnitude;

			// calculate how much distance we need to travel during one orbit
			this.orbitDisplacement = (2d*Math.PI*this.orbitRadius);
		}

		public override void Tick(ulong tick)
		{
			double distanceTraveled = Speed*tick;
			double radiansTravelled = (distanceTraveled/this.orbitDisplacement)*PIx2;

			// how far we've moved
			var delta = new Vector(Math.Cos(radiansTravelled)*this.orbitRadius, Math.Sin(radiansTravelled)*this.orbitRadius, 0);

			// update our base classes position, we don't want to have to recalculate our orbit factors
			base.UniversalCoordinates = Orbiting.UniversalCoordinates + delta;
		}
	}
}