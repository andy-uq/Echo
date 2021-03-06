﻿using System.Collections.Generic;
using System.Linq;

namespace Echo.State
{
	public class UniverseState : IObjectState
	{
		public ulong ObjectId { get; set; }
		public string Name { get; set; }

		public IEnumerable<StarClusterState> StarClusters { get; set; }
		public IEnumerable<CorporationState> Corporations { get; set; }
		public IEnumerable<SkillInfo> Skills { get; set; }
		public IEnumerable<ItemInfo> Items { get; set; }
		public IEnumerable<WeaponInfo> Weapons { get; set; }
		public IEnumerable<ShipInfo> Ships { get; set; }
		public IEnumerable<BluePrintInfo> BluePrints { get; set; }

		public ulong NextId { get; set; }

		public UniverseState()
		{
			StarClusters = Enumerable.Empty<StarClusterState>();
			Corporations = Enumerable.Empty<CorporationState>();
			Skills = Enumerable.Empty<SkillInfo>();
			Items = Enumerable.Empty<ItemInfo>();
			Weapons = Enumerable.Empty<WeaponInfo>();
			Ships = Enumerable.Empty<ShipInfo>();
			BluePrints = Enumerable.Empty<BluePrintInfo>();
		}
	}
}