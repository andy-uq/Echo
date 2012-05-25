using System.Collections.Generic;
using Echo;

namespace Echo.State
{
	public class SolarSystemState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public List<CelestialObjectState> Satellites { get; set; }
		public List<StructureState> Structures { get; set; }
		public List<ShipState> Ships { get; set; }
	}		 
}