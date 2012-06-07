using System.Collections.Generic;
using Echo.Celestial;
using Echo;
using Echo.Corporations;
using Echo.Market;

namespace Echo.Structures
{
	public abstract partial class Structure : OrbitingObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Structure; }
		}

		public abstract StructureType StructureType { get; }

		public Corporation Owner { get; set; }
		public List<SellOrder> SellOrders { get; private set; }
		public List<BuyOrder> BuyOrders { get; private set; }

		protected Structure()
		{
			SellOrders = new List<SellOrder>();
			BuyOrders = new List<BuyOrder>();
		}
	}
}