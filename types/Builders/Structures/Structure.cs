using System;
using Echo.State;
using Echo;

namespace Echo.Structures
{
	partial class Structure
	{
		public abstract class Builder
		{
			public Structure Build(ILocation location, StructureState state)
			{
				var structure = BuildStructure(location, state);
				structure.Id = state.Id;
				structure.Name = state.Name;
				structure.Position = new Position(location, state.LocalCoordinates);

				return structure;
			}

			protected abstract Structure BuildStructure(ILocation location, StructureState state);

			public static Builder For(StructureState state)
			{
				if (state.Manufactory != null)
					return new Manufactory.Builder();

				throw new InvalidOperationException("Cannot determine builder for Structure");
			}
		}
	}
}