using Echo.State;

namespace Echo.Ships
{
	public partial class Weapon : ILocation
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Item; }
		}

		public long Id { get; private set; }
		public string Name { get; private set; }
		public Position Position { get; set; }

		public HardPoint HardPoint { get; set; }
		public WeaponInfo WeaponInfo { get; set; }
	}
}