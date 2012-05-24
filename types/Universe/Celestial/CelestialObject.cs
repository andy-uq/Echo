using System.Collections.Generic;
using Echo.Structures;
using Echo;

namespace Echo.Celestial
{
	public partial class CelestialObject : OrbitingObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CelestialObject; }
		}

		public virtual CelestialObjectType CelestialObjectType { get { return CelestialObjectType.Object; }}
		public double Mass { get; private set; }
		public double Size { get; set; }
		public List<CelestialObject> Satellites { get; private set; }
		public List<Structure> Structures { get; private set; }

		public CelestialObject()
		{
			Satellites = new List<CelestialObject>();
			Structures = new List<Structure>();
		}
	}
}