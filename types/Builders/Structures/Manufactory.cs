using Echo.State;
using Echo;

namespace Echo.Structures
{
	partial class Manufactory
	{
		public new class Builder : Structure.Builder
		{
			protected override Structure BuildStructure(ILocation location, StructureState state)
			{
				var structure = new Manufactory
				{
					Efficiency = state.Manufactory.Efficiency
				};

				return structure;
			}
		}
	}
}