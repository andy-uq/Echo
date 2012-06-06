using Echo.Statistics;

namespace Echo.Ships
{
	public class ArmourPlating : IStatisticDelta<double>
	{
		public ShipStatistic ArmourType { get; set; }
		public double Value { get; set; }

		public ArmourPlating(ShipStatistic armourType)
		{
			ArmourType = armourType;
		}
	}
}