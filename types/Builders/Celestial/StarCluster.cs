using System.Linq;
using Echo.State;
using Echo;

namespace Echo.Celestial
{
	partial class StarCluster
	{
		 public class Builder
		 {
			 public StarCluster Build(Universe universe, StarClusterState state)
			 {
			 	var starCluster = new StarCluster
			 	{
			 		Id = state.Id,
			 		Name = state.Name,
			 		Position = new Position(universe, state.LocalCoordinates),
			 	};

			 	var solarSystemBuilder = new SolarSystem.Builder();
			 	starCluster.SolarSystems = state.SolarSystems
					.Select(x => solarSystemBuilder.Build(starCluster, x))
			 		.ToList();

				 return starCluster;
			 }
		 }
	}
}