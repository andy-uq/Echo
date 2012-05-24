using System.Collections.Generic;
using Echo;

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

		public long Id { get; set; }

		public string Name { get; set; }

		public List<SolarSystem> SolarSystems { get; set; }

		public Position Position { get; set; }

		public void Tick(ulong tick)
		{
		}
	}
}