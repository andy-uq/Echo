using Echo.Corporations;
using Echo.State;

namespace Echo.Items
{
	public partial class Item : IObject
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Item; }
		}

		public Item(ItemInfo itemInfo, uint quantity = 1)
		{
			ItemInfo = itemInfo;
			Quantity = quantity;
		}

		private Item()
		{
		}

		public long Id { get; private set; }
		public string Name { get { return ItemInfo.Name; } }

		public ItemInfo ItemInfo { get; private set; }
		
		public uint Quantity { get; set; }
		public Corporation Owner { get; set; }
	}
}