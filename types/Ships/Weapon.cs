using Echo.Items;
using Echo.State;

namespace Echo.Ships
{
	public partial class Weapon : ILocation
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Weapon; }
		}

		public ulong Id { get; private set; }
		public string Name { get { return WeaponInfo.Name; } }

		public Position Position { get; set; }

		public HardPoint HardPoint { get; set; }
		public WeaponInfo WeaponInfo { get; set; }
	}
}