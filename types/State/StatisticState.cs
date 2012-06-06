namespace Echo.State
{
	public class StatisticState<TValue>
	{
		public TValue Value { get; set; }
		public TValue CurrentValue { get; set; }
	}
}