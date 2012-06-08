using System;
using Echo.Items;
using Echo.Ships;

namespace Echo.State
{
	public class ShipInfo : IObject, IObjectState
	{
		public Guid Id { get; set; }
		public ItemCode Code { get; set; }
		public ShipClass ShipClass { get; set; }
		public SkillLevel[] PilotRequirements { get; set; }
		public ItemCode BluePrint { get; set; }

		public string Name { get; set; }

		public ObjectType ObjectType
		{
			get { return ObjectType.Info; }
		}

		long IObject.Id
		{
			get { return ObjectId; }
		}

		public long ObjectId
		{
			get { return Code.ToId(); }
		}
	}
}