using System;
using System.Collections.Generic;

namespace Echo.State
{
	public class SolarSystemState : IObjectState
	{
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public IEnumerable<CelestialObjectState> Satellites { get; set; }
		public IEnumerable<StructureState> Structures { get; set; }
		public IEnumerable<ShipState> Ships { get; set; }
		public IEnumerable<JumpGateState> JumpGates { get; set; }
	}		 
}