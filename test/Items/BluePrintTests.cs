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
		private readonly MockUniverse _universe = new MockUniverse();
		private Agent _agent;

		private Agent Agent
		{
			get { return _agent; }
		}

		private BluePrintInfo BluePrint
		{
			get { return _universe.BluePrint; }
		}

		[SetUp]
		public void BuildAgent()
		{
			var builder = Agent.Builder.Build(_universe.John);
			builder.RegisterTestSkills();

			_agent = builder.Materialise();
		}

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
			Assert.That(Agent.Skills.Keys, Contains.Item(SkillCode.SpaceshipCommand));
			Assert.That(Agent.Skills[SkillCode.SpaceshipCommand].Level, Is.GreaterThanOrEqualTo(5));

			Assert.That(Agent.CanUse(BluePrint), Is.True);
		}

		[Test]
		public void HaveMaterials()
		{
			Assert.That(BluePrint.HasMaterials(new ItemState[0]), Is.False);
			Assert.That(BluePrint.HasMaterials(BluePrint.Materials), Is.True);
			Assert.That(BluePrint.HasMaterials(new[] { new ItemState() { Code = ItemCode.LightFrigate, Quantity = 1 } }), Is.False);
			Assert.That(BluePrint.HasMaterials(new[] { new ItemState() { Code = ItemCode.Veldnium, Quantity = 5 } }), Is.False);
			Assert.That(BluePrint.HasMaterials(new[] { new ItemState() { Code = ItemCode.Veldnium, Quantity = 5 }, new ItemState() { Code = ItemCode.Veldnium, Quantity = 5 } }), Is.True);
		}
	}
}