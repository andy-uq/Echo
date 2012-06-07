using System.Collections.Generic;
using Echo.Items;

namespace Echo.State
{
	public class BluePrintInfo : IItemInfo
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

		public IEnumerable<ItemState> Materials { get; set; }
		public SkillLevel[] BuildRequirements { get; set; }
	}
}