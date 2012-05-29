using Echo.State;

namespace Echo.Structures
{
	partial class TradingStation
	{
		 public new class Builder : Structure.Builder
		 {
		 	protected override Structure BuildStructure(ILocation location, StructureState state)
		 	{
				var structure = new TradingStation
				{
				};

				return structure;
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