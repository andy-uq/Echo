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

		public Item(Item item, uint quantity, Corporation newOwner)
		{
			ItemInfo = item.ItemInfo;
			Quantity = quantity;
			Location = item.Location;
			Owner = newOwner;
		}

		public ulong Id { get; private set; }
		public string Name { get { return ItemInfo.Name; } }

		public ItemInfo ItemInfo { get; private set; }
		
		public uint Quantity { get; set; }
		public Corporation Owner { get; set; }

		public ILocation Location { get; set; }
	}
}