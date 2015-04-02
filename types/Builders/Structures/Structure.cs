using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Builder;
using Echo.Builders;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.State;
using Echo;
using EnsureThat;

namespace Echo.Structures
{
	partial class Structure
	{
		public abstract class Builder
		{
			protected StructureState State { get; set; }

			protected Builder(StructureState state)
			{
				State = state;
			}

			protected Builder()
			{
			}

			public StructureState Save(Structure structure)
			{
				Ensure.That(structure, "structure").IsNotNull();
				Ensure.That(structure.Position.Location, "structure.Position.Location").IsNotNull();

				State = new StructureState
				{
					ObjectId = structure.Id,
					Name = structure.Name,
					LocalCoordinates = structure.Position.LocalCoordinates,
					Owner = structure.Owner.ToObjectReference(),
					Orbits = structure.Position.Location.ToObjectReference(),
					StructureType = structure.StructureType,
					Personnel = structure.Personnel.Select(p => p.ToObjectReference()),
					HangerItems = structure.Hangar.Where(x => !x.Value.IsEmpty).Select(SaveHangarItems),
					BuyOrders = structure.BuyOrders.Select(_ => _.Save()),
					SellOrders = structure.SellOrders.Select(_ => _.Save()),
				};

				return SaveStructure(structure);
			}

			public ObjectBuilder<Structure> Build(ILocation location)
			{
				var builder = BuildStructure(location);
				builder.Target.Id = State.ObjectId;
				builder.Target.Name = State.Name;
				builder.Target.Position = new Position(location, State.LocalCoordinates);

				builder.Resolve((resolver, target) => target.Owner = resolver.Get<Corporation>(State.Owner));
				builder.Resolve((resolver, target) => LoadHangarItems(resolver, target, State.HangerItems));
				builder.Resolve((resolver, target) => target.Personnel.AddRange(State.Personnel.Select(resolver.Get<Agent>)));

				builder
					.Dependents(State.BuyOrders)
					.Build(x => BuyOrder.Builder.Build(builder.Target, x, () => new BuyOrder()))
					.Resolve((resolver, target, buyOrder) => target.BuyOrders.Add(buyOrder));

				builder
					.Dependents(State.SellOrders)
					.Build(x => SellOrder.Builder.Build(builder.Target, x, () => new SellOrder()))
					.Resolve((resolver, target, buyOrder) => target.SellOrders.Add(buyOrder));

				return builder;
			}

			private HangarItemState SaveHangarItems(KeyValuePair<Corporation, ItemCollection> hangarItems)
			{
				return new HangarItemState
				{
					Owner = hangarItems.Key.ToObjectReference(), 
					Items = hangarItems.Value.Select(_ => _.Save())
				};
			}

			private void LoadHangarItems(IIdResolver idresolver, Structure target, IEnumerable<HangarItemState> hangerItems)
			{
				if (target.Owner == null)
					throw new InvalidOperationException("Cannot load hangar items without owner");

				foreach (var hangar in hangerItems)
				{
					var corporation = idresolver.Get<Corporation>(hangar.Owner);
					var items = hangar.Items.Select(i => Item.Builder.Build(i, idresolver));
					var itemCollection = new ItemCollection(corporation.Property, items);
					
					target.Hangar.Add(corporation, itemCollection);
				}
			}

			protected abstract ObjectBuilder<Structure> BuildStructure(ILocation location);
			protected abstract StructureState SaveStructure(Structure structure);

			public static Builder For(Structure structure)
			{
				if (structure is Manufactory)
					return new Manufactory.Builder();

				if (structure is TradingStation)
					return new TradingStation.Builder();

				throw new InvalidOperationException("Cannot determine builder for Structure");
			}

			public static Builder For(StructureState state)
			{
				if (state.Manufactory != null)
					return new Manufactory.Builder(state);

				if (state.TradingStation != null)
					return new TradingStation.Builder(state);

				throw new InvalidOperationException("Cannot determine builder for Structure");
			}
		}
	}
}