using Echo.Builder;
using Echo.State;
using Echo;

namespace Echo.Structures
{
	partial class Manufactory
	{
		public new class Builder : Structure.Builder
		{
			public Builder() : base()
			{
			}

			public Builder(StructureState state) : base(state)
			{
				
			}

			protected override ObjectBuilder<Structure> BuildStructure(ILocation location)
			{
				var structure = new Manufactory
				{
					Efficiency = State.Manufactory.Efficiency
				};

				return new ObjectBuilder<Structure>(structure);
			}

			protected override StructureState SaveStructure(Structure structure)
			{
				var manufactory = (Manufactory) structure;
				State.Manufactory = new ManufactoryState
				{
					Efficiency = manufactory.Efficiency,
				};

				return State;
			}
		}
	}
}