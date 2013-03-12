using System;
using System.Linq;
using Echo.Builder;
using Echo.Corporations;
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
					Orbits = structure.Position.Location.ToObjectReference(),
					StructureType = structure.StructureType,
					BuyOrders = structure.BuyOrders.Select(BuyOrder.Builder.Save),
					SellOrders = structure.SellOrders.Select(SellOrder.Builder.Save),
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
				builder.Resolve(LoadHangarItems);
				
				builder
					.Dependents(State.BuyOrders)
					.Build(x => BuyOrder.Builder.Build(builder.Target, x))
					.Resolve((resolver, target, buyOrder) => target.BuyOrders.Add(buyOrder));
				
				builder
					.Dependents(State.SellOrders)
					.Build(x => SellOrder.Builder.Build(builder.Target, x))
					.Resolve((resolver, target, buyOrder) => target.SellOrders.Add(buyOrder));

				return builder;
			}

			private void LoadHangarItems(IIdResolver idresolver, Structure target)
			{
				if (target.Owner == null)
					throw new InvalidOperationException("Cannot load hangar items without owner");
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