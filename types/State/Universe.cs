using System.Collections.Generic;
using System.Linq;
using Echo;

namespace Echo.State
{
	public class UniverseState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<StarClusterState> StarClusters { get; set; }
		public IEnumerable<CorporationState> Corporations { get; set; }
		public IEnumerable<SkillInfo> Skills { get; set; }
		public IEnumerable<ItemInfo> Items { get; set; }
		public IEnumerable<ShipInfo> Ships { get; set; }

		public UniverseState()
		{
			StarClusters = Enumerable.Empty<StarClusterState>();
			Corporations = Enumerable.Empty<CorporationState>();
			Skills = Enumerable.Empty<SkillInfo>();
			Items = Enumerable.Empty<ItemInfo>();
			Ships = Enumerable.Empty<ShipInfo>();
		}
	}
}