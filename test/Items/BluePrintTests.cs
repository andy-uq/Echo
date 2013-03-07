using System;
using System.Collections.Generic;
using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.State;
using Echo.Tests.Mocks;
using NUnit.Framework;
using SkillLevel = Echo.State.SkillLevel;
using AgentSkillLevel = Echo.Agents.Skills.SkillLevel;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class BluePrintTests
	{
		[Test]
		public void NewBluePrint()
		{
			var bluePrint = new BluePrintInfo();
			Assert.That(bluePrint.BuildRequirements, Is.Empty);
			Assert.That(bluePrint.Materials, Is.Empty);
		}

		[Test]
		public void CanOnlyUseBluePrintItemCodes()
		{
			new BluePrintInfo(ItemCode.MiningLaser);
			try
			{
				new BluePrintInfo(ItemCode.Veldnium);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void AgentCanBuild()
		{
			var bp = new BluePrintInfo(ItemCode.MiningLaser)
			{
				BuildRequirements = new[] {new SkillLevel {SkillCode = SkillCode.SpaceshipCommand, Level = 5},},
				Materials = new[] { new ItemState { Code = ItemCode.Veldnium, Quantity = 10 }, }
			};

			var a1 = Agent.Builder.Build(new AgentState()).Build(IdResolutionContext.Empty);
			a1.Skills.Add(new AgentSkillLevel() { Skill = TestSkills.For(SkillCode.SpaceshipCommand), Level = 5 });

			Assert.That(a1.CanUse(bp), Is.True);
		}

		[Test]
		public void HaveMaterials()
		{
			var bp = new BluePrintInfo(ItemCode.MiningLaser)
			{
				BuildRequirements = new[] {new SkillLevel {SkillCode = SkillCode.SpaceshipCommand, Level = 5},},
				Materials = new[] { new ItemState { Code = ItemCode.Veldnium, Quantity = 10 }, }
			};

			Assert.That(bp.HasMaterials(new ItemState[0]), Is.False);
			Assert.That(bp.HasMaterials(bp.Materials), Is.True);
			Assert.That(bp.HasMaterials(new[] { new ItemState() { Code = ItemCode.LightFrigate, Quantity = 1 } }), Is.False);
			Assert.That(bp.HasMaterials(new[] { new ItemState() { Code = ItemCode.Veldnium, Quantity = 5 } }), Is.False);
			Assert.That(bp.HasMaterials(new[] { new ItemState() { Code = ItemCode.Veldnium, Quantity = 5 }, new ItemState() { Code = ItemCode.Veldnium, Quantity = 5 } }), Is.True);
		}
	}
}