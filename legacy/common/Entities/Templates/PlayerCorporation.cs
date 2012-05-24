using System;

using Echo.Objects;

namespace Echo.Entities
{
	public partial class PlayerCorporation
	{
		public class PlayerTemplate : CorporationTemplate<PlayerCorporation>
		{
			public PlayerTemplate(ObjectFactory universe) : base(universe)
			{
			}

			protected override PlayerCorporation Create()
			{
				var player = base.Create();
				player.CreateDate = DateTime.Now;

				return player;
			}
		}
	}
}