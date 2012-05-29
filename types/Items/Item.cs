using Echo.Corporations;

namespace Echo.Items
{
	public partial class Item : IItem
	{
		public long _ownerId;

		public ObjectType ObjectType
		{
			get { return ObjectType.Item; }
		}

		public long Id { get; private set; }
		public string Name { get; private set; }
		public Position Position { get; set; }

		public void Tick(ulong tick)
		{
		}

		public Corporation Owner { get; set; }
		public uint Quantity { get; set; }
	}
}