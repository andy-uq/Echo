using System;
using Echo.Items;
using Echo.Ships;

namespace Echo.State
{
	public class ShipInfo : IObject, IObjectState
	{
		public string Id { get; set; }
		public ItemCode Code { get; set; }
		public ShipClass ShipClass { get; set; }
		public SkillLevel[] PilotRequirements { get; set; }
		public ItemCode BluePrint { get; set; }

		#region IObject Members

		public string Name { get; set; }

		public ObjectType ObjectType
		{
			get { return ObjectType.Info; }
		}

		long IObject.Id
		{
			get { return ObjectId; }
		}

		#endregion

		#region IObjectState Members

		public long ObjectId
		{
			get { return Code.ToId(); }
		}

		#endregion
	}
}