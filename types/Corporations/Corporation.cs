using System.Collections.Generic;
using Echo.Items;
using Echo.Market;
using Echo.Ships;
using Echo.Structures;

namespace Echo.Corporations
{
	public partial class Corporation : IObject
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Corporation; }
		}

		public long Id { get; set; }
		public string Name { get; set; }
		
		public List<Item> Property { get; set; }
		public List<Ship> Ships { get; set; }
		public List<Structure> Structures { get; set; }
		public List<Actor> Employees { get; set; }
		public List<BuyOrder> BuyOrders { get; set; }
		public List<SellOrder> SellOrders { get; set; }
		public List<Item> BluePrints { get; set; }
		public List<Item> Assets { get; set; }
	}
}