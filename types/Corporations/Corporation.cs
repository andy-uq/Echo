using System.Collections.Generic;
using Echo.Agents;
using Echo.Items;
using Echo.Market;
using Echo.Ships;
using Echo.State;
using Echo.Structures;

namespace Echo.Corporations
{
	public partial class Corporation : IObject
	{
		public Corporation()
		{
			Employees = new List<Agent>();
			Property = new ItemCollection();
			Structures = new List<Structure>();
			Ships = new List<Ship>();

			BuyOrders = new List<BuyOrder>();
			SellOrders = new List<SellOrder>();
			BluePrints = new List<Item>();
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.Corporation; }
		}

		public ulong Id { get; set; }
		public string Name { get; set; }
		
		public ItemCollection Property { get; set; }
		public List<Ship> Ships { get; set; }
		public List<Structure> Structures { get; set; }
		public List<Agent> Employees { get; set; }
		public List<BuyOrder> BuyOrders { get; set; }
		public List<SellOrder> SellOrders { get; set; }
		public List<Item> BluePrints { get; set; }

		public ItemCollection GetProperty(Structure structure)
		{
			ItemCollection property;
			if (!structure.Hangar.TryGetValue(this, out property))
			{
				property = new ItemCollection();
				structure.Hangar[this] = property;
			}

			return property;
		}
	}
}