using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Statistics;
using NUnit.Framework;

namespace Echo.Tests.Skills
{
	[TestFixture]
	public class SkillTests
	{
		[Test]
		public void SpaceshipCommand()
		{
			var s = new Skill
			{
				Code = SkillCode.SpaceshipCommand,
				Name = "Spaceship Command",
				PrimaryStat = AgentStatistic.Perception,
				SecondaryStat = AgentStatistic.Willpower,
				TrainingMultiplier = 1,
				Prerequisites = new SkillLevel[0],
				
			}
		}
	}
}