﻿using System.Collections.Generic;
using System.Linq;

namespace Echo.State
{
	public class StarClusterState : IObjectState
	{
		public StarClusterState()
		{
			SolarSystems = Enumerable.Empty<SolarSystemState>();
		}

		public string Id { get; set; }
		public ulong ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public IEnumerable<SolarSystemState> SolarSystems { get; set; }
		public MarketPlaceState MarketPlace { get; set; }
	}
}