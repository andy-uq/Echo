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

		public DamageType DamageType { get; set; }

		public double Speed { get; set; }
		public int MaximumDamage { get; set; }
		public int MinimumDamage { get; set; }
	}
}