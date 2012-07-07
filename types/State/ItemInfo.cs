using System;
using Echo.Items;

namespace Echo.State
{
	public class ItemInfo : IObjectState, IObject
	{
		public Guid Id { get; set; }
		public ItemCode Code { get; set; }

		#region IObject Members

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
}