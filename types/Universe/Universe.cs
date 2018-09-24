using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Skills;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Items;
using Echo.State;

namespace Echo
{
	public static class UniverseExtensions
	{
		public static IEnumerable<Planet> Planets(this Universe u) => u
			.SolarSystems()
			.SelectMany(x => x.Satellites.OfType<Planet>());

		public static IEnumerable<SolarSystem> SolarSystems(this Universe u) => u
			.StarClusters
			.SelectMany(starCluster => starCluster.SolarSystems);
	}

	public partial class Universe : IObject, ILocation
	{
		public Universe()
		{
			StarClusters = new List<StarCluster>();
			Corporations = new List<Corporation>();
			Items = new Dictionary<ItemCode, ItemInfo>();
			Weapons = new Dictionary<ItemCode, WeaponInfo>();
			BluePrints = new Dictionary<ItemCode, BluePrintInfo>();
			Skills = new Dictionary<SkillCode, SkillInfo>();
			Ships = new Dictionary<ItemCode, ShipInfo>();
		}

		public ObjectType ObjectType => ObjectType.Universe;

		public ulong Id => 1L;

		public string Name => "The Universe";

		public Vector LocalCoordinates => Vector.Zero;

		public List<StarCluster> StarClusters { get; }
		public List<Corporation> Corporations { get; }
		public Dictionary<ItemCode, ItemInfo> Items { get; }
		public Dictionary<ItemCode, WeaponInfo> Weapons { get; }
		public Dictionary<ItemCode, BluePrintInfo> BluePrints { get; }
		public Dictionary<ItemCode, ShipInfo> Ships { get; }
		public Dictionary<SkillCode, SkillInfo> Skills { get; }

		public Position Position => new Position();
	}
}