using Echo.Builder;
using Echo.State;

namespace Echo.Structures
{
	partial class TradingStation
	{
		public new class Builder : Structure.Builder
		{
			public Builder()
			{
			}

			public Builder(StructureState state)
				: base(state)
			{
			}


			protected override ObjectBuilder<Structure> BuildStructure(ILocation location)
			{
				var structure = new TradingStation();

				return new ObjectBuilder<Structure>(structure);
			}

			protected override StructureState SaveStructure(Structure structure)
			{
				var tradingStation = (TradingStation)structure;
				State.TradingStation = new TradingStationState();

				return State;
			}
		}
	}
}