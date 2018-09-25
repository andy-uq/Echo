using Echo.Statistics;

namespace Echo.Ships
{
	public class ArmourDelta : IStatisticDelta<double>
	{
		public ShipStatistic ArmourType { get; }
		public string Name { get; set; }
		public double Value { get; set; }

		public ArmourDelta(ShipStatistic armourType)
		{
			ArmourType = armourType;
		}
	}
}