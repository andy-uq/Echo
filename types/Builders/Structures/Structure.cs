using System;
using System.Linq;
using Echo.Builder;
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
			public StructureState Save(Structure structure)
			{
				Ensure.That(structure, "structure").IsNotNull();

				var state = new StructureState
				{
					Id = structure.Id,
					Name = structure.Name,
					LocalCoordinates = structure.Position.LocalCoordinates,
					Orbits = structure.Position.Location.AsObjectReference(),
					StructureType = structure.StructureType,
					BuyOrders = structure.BuyOrders.Select(BuyOrder.Builder.Save),
					SellOrders = structure.SellOrders.Select(SellOrder.Builder.Save),
				};

				return SaveStructure(structure, state);
			}

			public ObjectBuilder<Structure> Build(ILocation location, StructureState state)
			{
				var builder = BuildStructure(location, state);
				builder.Target.Id = state.Id;
				builder.Target.Name = state.Name;
				builder.Target.Position = new Position(location, state.LocalCoordinates);

				builder
					.Dependents(state.BuyOrders)
					.Build(x => BuyOrder.Builder.Build(builder.Target, x))
					.Resolve((resolver, target, buyOrder) => target.BuyOrders.Add(buyOrder));
				
				builder
					.Dependents(state.SellOrders)
					.Build(x => SellOrder.Builder.Build(builder.Target, x))
					.Resolve((resolver, target, buyOrder) => target.SellOrders.Add(buyOrder));

				return builder;
			}

			protected abstract ObjectBuilder<Structure> BuildStructure(ILocation location, StructureState state);
			protected abstract StructureState SaveStructure(Structure structure, StructureState state);

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
					return new Manufactory.Builder();

				if (state.TradingStation != null)
					return new TradingStation.Builder();

				throw new InvalidOperationException("Cannot determine builder for Structure");
			}
		}
	}
}