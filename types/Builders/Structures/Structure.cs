using System;
using Echo.State;
using Echo;

namespace Echo.Structures
{
	partial class Structure
	{
		public abstract class Builder
		{
			public StructureState Save(Structure structure)
			{
				var state = new StructureState
				{
					Id = structure.Id,
					Name = structure.Name,
					LocalCoordinates = structure.Position.LocalCoordinates,
					OrbitsId = structure.Position.Location.Id,
					StructureType = structure.StructureType,
				};

				return SaveStructure(structure, state);
			}

			public Structure Build(ILocation location, StructureState state)
			{
				var structure = BuildStructure(location, state);
				structure.Id = state.Id;
				structure.Name = state.Name;
				structure.Position = new Position(location, state.LocalCoordinates);

				return structure;
			}

			protected abstract Structure BuildStructure(ILocation location, StructureState state);
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