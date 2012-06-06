using System;
using Echo.Builder;
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
					OrbitsId = (structure.Position.Location == null) ? -1L : structure.Position.Location.Id,
					StructureType = structure.StructureType,
				};

				return SaveStructure(structure, state);
			}

			public ObjectBuilder<Structure> Build(ILocation location, StructureState state)
			{
				var builder = BuildStructure(location, state);
				builder.Target.Id = state.Id;
				builder.Target.Name = state.Name;
				builder.Target.Position = new Position(location, state.LocalCoordinates);

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