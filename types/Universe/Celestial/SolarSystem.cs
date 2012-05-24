using System.Collections.Generic;
using Echo.Structures;
using Echo;

namespace Echo.Celestial
{
	public partial class SolarSystem : ILocation, IMoves
	{
		public ObjectType ObjectType { get { return ObjectType.SolarSystem;} }
		public long Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }

		public List<CelestialObject> Satellites { get; set; } 
		public List<Structure> Structures { get; set; } 

		public void Tick(ulong tick)
		{
		}
	}
}