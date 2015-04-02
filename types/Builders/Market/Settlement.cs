using System.Linq;
using Echo.Builders;
using Echo.State.Market;

namespace Echo.Market
{
	partial class Settlement
	{
		public static class Builder
		{
			public static SettlementState Save(Settlement settlement)
			{
				return new SettlementState
				{
					Item = settlement.Item.Save(),
					Owner = settlement.Item.Owner.ToObjectReference(),
					TimeToSettlement = settlement.Timer,
					SpendByOwner = settlement._spendByOwner.ToDictionary(k => k.Key.ToObjectReference(), v => v.Value),
				};
			}
		}
	}
}