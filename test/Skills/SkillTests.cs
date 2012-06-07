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
		public void ParseSkillCode()
		{
			SkillCode skillCode;

			const long badId = 1L;
			Assert.That(badId.TryParse(out skillCode), Is.False);

			const long goodId = (long )SkillCode.SpaceshipCommand ^ SkillCodeExtensions.SKILL_ID_MASK;
			Assert.That(goodId.TryParse(out skillCode), Is.True);
			Assert.That(skillCode, Is.EqualTo(SkillCode.SpaceshipCommand));
		}

		[Test]
		public void SkillCodeToId()
		{
			const SkillCode skillCode = SkillCode.SpaceshipCommand;
			Assert.That(skillCode.ToId(), Is.EqualTo(1073741825));
		}

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

			};
		}
	}
}