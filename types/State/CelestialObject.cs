using System;
using Echo;

namespace Echo.State
{
	public class CelestialObjectState : IObjectState
	{
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }

		public CelestialObjectType CelestialObjectType { get; set; }
		public double Size { get; set; }
		public double Mass { get; set; }
		public ObjectReference? Orbits { get; set; }

		public AsteroidBeltState AsteroidBelt { get; set; }
	}
}