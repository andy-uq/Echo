using System.Collections.Generic;
using Echo.Agents;
using Echo.Celestial;
using Echo;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.Tasks;

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

		public HashSet<ITask> Tasks { get; set; }
		public List<Agent> Personnel { get; set; }

		public Dictionary<Corporation, ItemCollection> Hangar { get; private set; }

		protected Structure()
		{
			SellOrders = new List<SellOrder>();
			BuyOrders = new List<BuyOrder>();
			Hangar = new Dictionary<Corporation, ItemCollection>();
			Personnel = new List<Agent>();
			Tasks = new HashSet<ITask>();
		}

		public void RegisterTask(ITask task)
		{
			Tasks.Add(task);
		}

		public void TaskComplete(ITask task)
		{
			Tasks.Remove(task);
		}
	}
}