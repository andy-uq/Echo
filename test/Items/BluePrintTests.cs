using System;
using System.Collections.Generic;
using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.State;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using test.common;
using AgentSkillLevel = Echo.Agents.Skills.SkillLevel;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class BluePrintTests
	{
		private readonly MockUniverse _universe = new MockUniverse();

		private Agent Agent
		{
			get { return _universe.John.StandUp(); }
		}

		private BluePrintInfo BluePrint
		{
			get { return _universe.BluePrint; }
		}

		[Test]
		public void NewBluePrint()
		{
			var bluePrint = new BluePrintInfo();

			bluePrint.BuildRequirements.ShouldBeEmpty();
			bluePrint.Materials.ShouldBeEmpty();
		}

		[Test]
		public void CanOnlyUseBluePrintItemCodes()
		{
			Should.NotThrow(() => new BluePrintInfo(ItemCode.MiningLaser));
			Should.Throw<ArgumentException>(() => new BluePrintInfo(ItemCode.Veldnium));
		}

		[Test]
		public void AgentCanBuild()
		{
			Agent.Skills.ShouldContain(i => i.Skill.Code == SkillCode.SpaceshipCommand);
			Agent.Skills[SkillCode.SpaceshipCommand].Level.ShouldBeGreaterThanOrEqualTo(5);

			Agent.CanUse(BluePrint).ShouldBe(true);
		}

		[Test]
		public void HaveMaterials()
		{
			BluePrint.HasMaterials(new ItemCollection()).ShouldBe(false);

			var itemCollection = new ItemCollection(initialContents: BluePrint.Materials.Build());
			BluePrint.HasMaterials(itemCollection).ShouldBe(true);

			var c1 = new[] {new ItemState() {Code = ItemCode.LightFrigate, Quantity = 1}};
			itemCollection = new ItemCollection(initialContents: c1.Build());
			BluePrint.HasMaterials(itemCollection).ShouldBe(false);

			var c2 = new[] {new ItemState() {Code = ItemCode.Veldnium, Quantity = 5}};
			itemCollection = new ItemCollection(initialContents: c2.Build());
			BluePrint.HasMaterials(itemCollection).ShouldBe(false);

			var c3 = new[] {new ItemState() {Code = ItemCode.Veldnium, Quantity = 5}, new ItemState() {Code = ItemCode.Veldnium, Quantity = 5}};
			itemCollection = new ItemCollection(initialContents: c3.Build());
			BluePrint.HasMaterials(itemCollection).ShouldBe(true);
		}

		[Test]
		public void CanBuild()
		{
			var itemFactory = new Moq.Mock<IItemFactory>(MockBehavior.Strict);
			var expected = TestItems.BuildItem(_universe.BluePrint.Code, _universe.BluePrint.TargetQuantity);
			itemFactory.Setup(f => f.Build(_universe.BluePrint.Code, _universe.BluePrint.TargetQuantity)).Returns(expected);

			var item = BluePrint.Build(itemFactory.Object);
			item.ShouldBeSameAs(expected);
		}
	}
}