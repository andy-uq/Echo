using Echo.Items;

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
		public virtual ItemType Type { get; set; }

		ulong IObject.Id
		{
			get { return ObjectId; }
		}

		public virtual ObjectType ObjectType
		{
			get { return ObjectType.Info; }
		}

		#endregion

		#region IObjectState Members

		public string Name { get; set; }

		public ulong ObjectId
		{
			get { return Type.ToId(Code); }
		}

		#endregion
	}
}