using Echo.Statistics;

namespace Echo.Ships
{
	public class ArmourDelta : IStatisticDelta<double>
	{
		public string Name { get; set; }
		public ShipStatistic ArmourType { get; set; }
		public double Value { get; set; }

		public ArmourDelta(ShipStatistic armourType)
		{
			ArmourType = armourType;
		}
	}
}