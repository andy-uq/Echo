using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Items;

namespace Echo.State
{
	public sealed class BluePrintInfo : ItemInfo
	{
		public BluePrintInfo(ItemCode code) : base(code)
		{
			if (!code.GetItemCategories().Any(c => c == ItemType.Blueprints))
				throw new ArgumentException("Cannot create a blueprint from this itemCode");

			Materials = Enumerable.Empty<ItemState>();
			BuildRequirements = new SkillLevel[0];
		}

		public BluePrintInfo()
		{
			Materials = Enumerable.Empty<ItemState>();
			BuildRequirements = new SkillLevel[0];
		}

		public IEnumerable<ItemState> Materials { get; set; }
		public SkillLevel[] BuildRequirements { get; set; }
		public uint TargetQuantity { get; set; }

		public bool HasMaterials(ItemCollection items)
		{
			return items.Contains(Materials.ToArray());
		}

		public Item Build(IItemFactory itemFactory)
		{
			return itemFactory.Build(Code, TargetQuantity);
		}

		public override ItemType Type
		{
			get { return ItemType.Blueprints; }
			set {  }
		}
	}
}