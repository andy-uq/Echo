using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo;

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
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.Universe; }
		}

		public long Id
		{
			get { return 1L; }
			set { }
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

		public void Tick(ulong tick)
		{
		}

		public string SystematicName
		{
			get { return Name; }
		}

		public Position Position
		{
			get { return new Position(); }
		}
	}
}