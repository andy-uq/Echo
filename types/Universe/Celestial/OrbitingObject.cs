namespace Echo.Celestial
{
	public abstract class OrbitingObject : ILocation, IMoves
	{
		public abstract ObjectType ObjectType { get; }
		public ulong Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }
	}
}