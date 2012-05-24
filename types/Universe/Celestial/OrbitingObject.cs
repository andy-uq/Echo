using Echo;

namespace Echo.Celestial
{
	public abstract partial class OrbitingObject : ILocation, IMoves
	{
		public abstract ObjectType ObjectType { get; }
		public long Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }
		
		public virtual void Tick(ulong tick)
		{
			
		}
	}
}