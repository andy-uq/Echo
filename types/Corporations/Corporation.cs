using System;
using System.Collections.Generic;
using Echo.Agents;
using Echo.Items;
using Echo.Market;
using Echo.Ships;
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

		public ObjectType ObjectType => ObjectType.Corporation;

		public ulong Id { get; set; }
		public string Name { get; set; }
		
		public ItemCollection Property { get; }
		public List<Ship> Ships { get; }
		public List<Structure> Structures { get; }
		public List<Agent> Employees { get; }
		public List<BuyOrder> BuyOrders { get; }
		public List<SellOrder> SellOrders { get; }
		public List<Item> BluePrints { get; }

		public ItemCollection GetProperty(Structure structure)
		{
			if (!structure.Hangar.TryGetValue(this, out var property))
			{
				property = new ItemCollection();
				structure.Hangar[this] = property;
			}

			return property;
		}

		public void Hire(Agent agent)
		{
			if (agent.Corporation != null)
				throw new InvalidOperationException("Unable able to hire an agent that's owned by another corporation");

			Employees.Add(agent);
			agent.Corporation = this;
		}

		public void Fire(Agent agent)
		{
			if (Employees.Remove(agent))
			{
				agent.Corporation = null;
			}
		}
	}
}