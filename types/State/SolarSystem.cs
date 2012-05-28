using System.Collections.Generic;
using Echo;

namespace Echo.State
{
	public class SolarSystemState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public IEnumerable<CelestialObjectState> Satellites { get; set; }
		public IEnumerable<StructureState> Structures { get; set; }
		public IEnumerable<ShipState> Ships { get; set; }
	}		 
}