using Echo.Builder;
using Echo.State;
using Echo;

namespace Echo.Structures
{
	partial class Manufactory
	{
		public new class Builder : Structure.Builder
		{
			protected override ObjectBuilder<Structure> BuildStructure(ILocation location, StructureState state)
			{
				var structure = new Manufactory
				{
					Efficiency = state.Manufactory.Efficiency
				};

				return new ObjectBuilder<Structure>(structure);
			}

			protected override StructureState SaveStructure(Structure structure, StructureState state)
			{
				var manufactory = (Manufactory) structure;
				state.Manufactory = new ManufactoryState
				{
					Efficiency = manufactory.Efficiency,
				};

				return state;
			}
		}
	}
}