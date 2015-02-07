using Echo.Agents;
using Echo.Agents.Skills;
using Echo.State;
using Echo.Statistics;
using NUnit.Framework;
using SkillLevel = Echo.State.SkillLevel;

namespace Echo.Tests.Skills
{
	[TestFixture]
	public class SkillTests
	{
		[Test]
		public void ParseSkillCode()
		{
			SkillCode skillCode;

			const ulong badId = 1L;
			Assert.That(badId.TryParse(out skillCode), Is.False);

			const ulong goodId = (long )SkillCode.SpaceshipCommand ^ SkillCodeExtensions.SKILL_ID_MASK;
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
			var s = new SkillInfo
			{
				Code = SkillCode.SpaceshipCommand,
				Name = "Spaceship Command",
				PrimaryStat = AgentStatistic.Perception,
				SecondaryStat = AgentStatistic.Willpower,
				TrainingMultiplier = 1,
				Prerequisites = new SkillLevel[0],

			};
		}

		[Test]
		public void GetSkillCategories()
		{
			var categories = SkillCode.SpaceshipCommand.GetSkillCategories();
			Assert.That(categories, Is.Not.Empty);
			Assert.That(categories, Is.EquivalentTo(new[] { SkillCategory.SpaceshipCommand }));
		}
	}
}