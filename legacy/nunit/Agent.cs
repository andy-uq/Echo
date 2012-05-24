using Echo.Entities;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class AgentTests
	{
		[Test]
		public void Stats()
		{
			var stat = new StatisticValue<AgentStatistic, int>(AgentStatistic.Memory, 100);
			Assert.IsFalse(stat.IsBuffed);
			Assert.IsFalse(stat.IsDebuffed);

			stat.Alter(110);
			Assert.IsTrue(stat > 100);
			Assert.IsTrue(stat.IsBuffed);
			Assert.IsFalse(stat.IsDebuffed);

			stat.Alter(90);
			Assert.IsTrue(stat < 100);
			Assert.IsFalse(stat.IsBuffed);
			Assert.IsTrue(stat.IsDebuffed);
		}
	}
}