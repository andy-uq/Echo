using System.Collections.Generic;
using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.Corporations;
using Echo.Items;
using Echo.State;
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
					ObjectId = settlement.Id,
					Name = settlement.Name,
					Item = settlement.Item.Save(),
					Owner = settlement.Item.Owner.ToObjectReference(),
					Location = settlement.Item.Location.ToObjectReference(),
					TimeToSettlement = settlement.Timer,
					SpendByOwner = settlement._spendByOwner.ToDictionary(k => k.Key.ToObjectReference(), v => v.Value)
				};
			}

			public static ObjectBuilder<Settlement> Build(SettlementState state)
			{
				var settlement = new Settlement
				{
					Id = state.ObjectId,
					Name = state.Name,
					Timer = state.TimeToSettlement
				};

				var builder = new ObjectBuilder<Settlement>(settlement);
				builder
					.Dependent(state.Item)
					.Build(item => Item.Builder.Build(item))
					.Resolve((resolver, target, item) => item.Location = resolver.Get<ILocation>(state.Location))
					.Resolve((resolver, target, item) => item.Owner = resolver.Get<Corporation>(state.Owner))
					.Resolve((resolver, target, item) => target._item = item);

				builder
					.Resolve((resolver, target) => SpendByOwner(resolver, target, state.SpendByOwner));

				return builder;
			}

			private static void SpendByOwner(IIdResolver resolver, Settlement settlement, Dictionary<ObjectReference, long> spendByOwner)
			{
				settlement._spendByOwner = spendByOwner.ToDictionary(spend => resolver.Get<Corporation>(spend.Key), spend => spend.Value);
			}
		}
	}
}