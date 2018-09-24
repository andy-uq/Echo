using System.Collections.Generic;
using System.Linq;
using Echo;
using Echo.State;

namespace site.code
{
	public static class StarClusterExtensions
	{
		public static Vector Size(this StarClusterState starCluster)
		{
			return Max(starCluster.SolarSystems.Select(x => x.LocalCoordinates));
		}

		public static Vector Size(this SolarSystemState solarSystem)
		{
			return Max(solarSystem.Satellites.Select(x => x.LocalCoordinates));
		}

		private static Vector Max(IEnumerable<Vector> vectors)
		{
			var extent = Vector.Zero;

			foreach (var vector in vectors)
			{
				if ( vector.Magnitude > extent.Magnitude )
					extent = vector;
			}

			return extent;
		}
	}
}