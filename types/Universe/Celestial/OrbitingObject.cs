using Echo;

namespace Echo.Celestial
{
	public abstract partial class OrbitingObject : ILocation, IMoves
	{
		public abstract ObjectType ObjectType { get; }
		public ulong Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }
	}
}