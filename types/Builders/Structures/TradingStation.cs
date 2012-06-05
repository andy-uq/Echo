using Echo.State;

namespace Echo.Structures
{
	partial class TradingStation
	{
		 public new class Builder : Structure.Builder
		 {
		 	protected override ObjectBuilder<Structure> BuildStructure(ILocation location, StructureState state)
		 	{
				var structure = new TradingStation
				{
				};

				return new ObjectBuilder<Structure>(structure);
			}

		 	protected override StructureState SaveStructure(Structure structure, StructureState state)
		 	{
				var tradingStation = (TradingStation)structure;
				state.TradingStation = new TradingStationState
				{
				};

				return state;
			}
		 }
	}
}