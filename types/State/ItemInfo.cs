using System;
using Echo.Items;

namespace Echo.State
{
	public sealed class ItemInfo : IObjectState, IObject
	{
		public Guid Id { get; set; }
		public ItemCode Code { get; set; }

		public string Name { get; set; }

		public long ObjectId
		{
			get { return Code.ToId(); }
		}

		long IObject.Id
		{
			get { return ObjectId; }
		}

		ObjectType IObject.ObjectType
		{
			get { return ObjectType.Info; }
		}
	}
}