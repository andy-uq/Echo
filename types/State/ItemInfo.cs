using Echo.Items;

namespace Echo.State
{
	public sealed class ItemInfo : IObjectState, IObject
	{
		public ItemCode Code { get; set; }
		public string Name { get; set; }

		public long Id
		{
			get { return Code.ToId(); }
			set { }
		}

		ObjectType IObject.ObjectType
		{
			get { return ObjectType.Info; }
		}
	}
}