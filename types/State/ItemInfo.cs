using Echo.Items;
using Echo.Statistics;

namespace Echo.State
{
	public class ItemInfo : IObjectState, IItemInfo
	{
		public ItemInfo()
		{
		}

		public ItemInfo(ItemCode code)
		{
			Code = code;
		}

		#region IItemInfo Members

		public string Id { get; set; }
		public ItemCode Code { get; set; }

		long IObject.Id
		{
			get { return ObjectId; }
		}

		ObjectType IObject.ObjectType
		{
			get { return ObjectType.Info; }
		}

		#endregion

		#region IObjectState Members

		public string Name { get; set; }

		public long ObjectId
		{
			get { return Code.ToId(); }
		}

		#endregion
	}

	public class ShieldInfo
	{
		public ShipStatistic Statistic { get; set; }
		public double RepairPerTick { get; set; }
	}
}