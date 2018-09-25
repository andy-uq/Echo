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
		public virtual ItemType Type { get; }

		ulong IObject.Id => ObjectId;

		public virtual ObjectType ObjectType => ObjectType.Info;

		#endregion

		#region IObjectState Members

		public string Name { get; set; }

		public ulong ObjectId => Type.ToId(Code);

		#endregion
	}
}