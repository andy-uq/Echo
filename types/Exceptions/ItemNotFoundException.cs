using System;
using System.Runtime.Serialization;

namespace Echo.Exceptions
{
	[Serializable]
	public class ItemNotFoundException : Exception
	{
		public string ItemType { get; }
		public object Item { get; }

		public ItemNotFoundException(string itemType, object item) : base($"Could not find {itemType} \"{item}\"")
		{
			ItemType = itemType;
			Item = item;
		}

		protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			ItemType = info.GetString("ItemType");
			Item = info.GetValue("Item", typeof (object));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ItemType", ItemType);
			info.AddValue("Item", Item);
		}
	}
}