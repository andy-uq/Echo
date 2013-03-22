using System;
using System.Collections.Generic;
using System.Linq;

namespace Echo.State
{
	public class SolarSystemState : IObjectState
	{
		public SolarSystemState()
		{
			Satellites = Enumerable.Empty<CelestialObjectState>();
			Structures = Enumerable.Empty<StructureState>();
			Ships = Enumerable.Empty<ShipState>();
			JumpGates = Enumerable.Empty<JumpGateState>();
		}

		public long ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public IEnumerable<CelestialObjectState> Satellites { get; set; }
		public IEnumerable<StructureState> Structures { get; set; }
		public IEnumerable<ShipState> Ships { get; set; }
		public IEnumerable<JumpGateState> JumpGates { get; set; }
	}		 
}