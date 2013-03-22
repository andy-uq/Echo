using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Echo.State;

namespace Echo.Web.Areas.Admin.Models
{
	public class StarClusterIndex
	{
		public IEnumerable<StarClusterState> StarClusters { get; set; }
		public StarCluster NewStarCluster { get; set; }
	}

	public class StarCluster
	{
		[Required]
		public string Name { get; set; }
		
		[Required]
		public Vector LocalCoordinates { get; set; }
		
		public double Extent { get; set; }
	}
}