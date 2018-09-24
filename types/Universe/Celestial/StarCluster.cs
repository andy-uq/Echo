using System.Collections.Generic;
using Echo.Market;

namespace Echo.Celestial
{
	public partial class StarCluster : ILocation
	{
		public StarCluster()
		{
			SolarSystems = new List<SolarSystem>();
		}

		public ObjectType ObjectType => ObjectType.StarCluster;

		public ulong Id { get; set; }

		public string Name { get; set; }

		public MarketPlace MarketPlace { get; set; }
		public List<SolarSystem> SolarSystems { get; }

		public Position Position { get; set; }
	}
}