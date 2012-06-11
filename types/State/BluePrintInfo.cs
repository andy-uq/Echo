using System.Collections.Generic;
using Echo.Items;

namespace Echo.State
{
	public sealed class BluePrintInfo : IObject, IItemInfo
	{
		public string Name { get; set; }
		public IEnumerable<ItemState> Materials { get; set; }
		public SkillLevel[] BuildRequirements { get; set; }

		#region IItemInfo Members

		public ItemCode Code { get; set; }

		#endregion

		#region IObject Members
		
		long IObject.Id
		{
			get { return Code.ToId(); }
		}

		ObjectType IObject.ObjectType
		{
			get { return ObjectType.Info; }
		}

		#endregion
	}
}