using System.Collections.Generic;
using Echo;
using Echo.Market;

namespace Echo.Celestial
{
	public partial class StarCluster : ILocation
	{
		public StarCluster()
		{
			SolarSystems = new List<SolarSystem>();
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.StarCluster; }
		}

		public ulong Id { get; set; }

		public string Name { get; set; }

		public MarketPlace MarketPlace { get; set; }
		public List<SolarSystem> SolarSystems { get; set; }

		public Position Position { get; set; }
	}
}