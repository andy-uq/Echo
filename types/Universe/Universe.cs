using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Skills;
using Echo.Celestial;
using Echo;
using Echo.Corporations;
using Echo.Items;
using Echo.State;

namespace Echo
{
	public static class UniverseExtensions
	{
		public static IEnumerable<Planet> Planets(this Universe u)
		{
			return u.SolarSystems().SelectMany(x => x.Satellites.OfType<Planet>());
		}

		public static IEnumerable<SolarSystem> SolarSystems(this Universe u)
		{
			return u.StarClusters.SelectMany(starCluster => starCluster.SolarSystems);
		}
	}

	public partial class Universe : IObject, ILocation
	{
		public Universe()
		{
			StarClusters = new List<StarCluster>();
			Corporations = new List<Corporation>();
			Items = new Dictionary<ItemCode, ItemInfo>();
			Skills = new Dictionary<SkillCode, SkillInfo>();
			Ships = new Dictionary<ItemCode, ShipInfo>();
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.Universe; }
		}

		public long Id
		{
			get { return 1L; }
		}

		public string Name
		{
			get { return "The Universe"; }
		}

		public Vector LocalCoordinates
		{
			get { return Vector.Zero; }
		}

		public List<StarCluster> StarClusters { get; private set; }
		public List<Corporation> Corporations { get; private set; }
		public Dictionary<ItemCode, ItemInfo> Items { get; private set; }
		public Dictionary<SkillCode, SkillInfo> Skills { get; private set; }
		public Dictionary<ItemCode, ShipInfo> Ships { get; private set; }

		public Position Position
		{
			get { return new Position(); }
		}

	}
}