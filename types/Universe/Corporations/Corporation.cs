using System.Collections.Generic;
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
		
		public List<IItem> Property { get; set; }
		public List<Ship> Ships { get; set; }
		public List<Structure> Structures { get; set; }
		public List<Actor> Employees { get; set; }
		public List<Auction> Auctions { get; set; }
		public List<IItem> BluePrints { get; set; }
		public List<IItem> Assets { get; set; }

		public void Tick(ulong tick)
		{
		}
	}
}