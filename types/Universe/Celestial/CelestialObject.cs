using System.Collections.Generic;
using Echo.Structures;

namespace Echo.Celestial
{
	public partial class CelestialObject : OrbitingObject
	{
		public override ObjectType ObjectType => ObjectType.CelestialObject;

		public virtual CelestialObjectType CelestialObjectType => CelestialObjectType.Object;
		public double Mass { get; private set; }
		public double Size { get; set; }
		public List<CelestialObject> Satellites { get; }
		public List<Structure> Structures { get; }

		public CelestialObject()
		{
			Satellites = new List<CelestialObject>();
			Structures = new List<Structure>();
		}
	}
}