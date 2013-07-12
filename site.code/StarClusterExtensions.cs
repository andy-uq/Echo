using System.Linq;
using Echo;
using Echo.State;

namespace site.code
{
	public static class StarClusterExtensions
	{
		public static Vector Size(this StarClusterState starCluster)
		{
			return starCluster.SolarSystems.Aggregate(Vector.Zero, (current, star) => current + (star.LocalCoordinates + star.Size()));
		}

		public static Vector Size(this SolarSystemState solarSystem)
		{
			return solarSystem.Satellites.Aggregate(Vector.Zero, (current, celestialBody) => current + (celestialBody.LocalCoordinates + new Vector(celestialBody.Size, 0)));
		}
	}
}