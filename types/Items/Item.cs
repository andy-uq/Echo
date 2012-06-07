using Echo.Corporations;
using Echo.State;

namespace Echo.Items
{
	public partial class Item 
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Item; }
		}

		public ItemInfo ItemInfo { get; private set; }
		
		public uint Quantity { get; set; }
		public Corporation Owner { get; set; }

		public void Tick(ulong tick)
		{
		}
	}
}