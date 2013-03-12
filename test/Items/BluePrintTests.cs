﻿using System;
using System.Collections.Generic;
using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.State;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;
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
			Assert.That(BluePrint.HasMaterials(new ItemCollection()), Is.False);

			var itemCollection = new ItemCollection(BluePrint.Materials.Build());
			Assert.That(BluePrint.HasMaterials(itemCollection), Is.True);

			var c1 = new[] {new ItemState() {Code = ItemCode.LightFrigate, Quantity = 1}};
			itemCollection = new ItemCollection(c1.Build());
			Assert.That(BluePrint.HasMaterials(itemCollection), Is.False);

			var c2 = new[] {new ItemState() {Code = ItemCode.Veldnium, Quantity = 5}};
			itemCollection = new ItemCollection(c2.Build());
			Assert.That(BluePrint.HasMaterials(itemCollection), Is.False);

			var c3 = new[] {new ItemState() {Code = ItemCode.Veldnium, Quantity = 5}, new ItemState() {Code = ItemCode.Veldnium, Quantity = 5}};
			itemCollection = new ItemCollection(c3.Build());
			Assert.That(BluePrint.HasMaterials(itemCollection), Is.True);
		}

		[Test]
		public void CanBuild()
		{
			var itemFactory = new Moq.Mock<IItemFactory>(MockBehavior.Strict);
			itemFactory.Setup(f => f.Build(_universe.BluePrint.Code, 1)).Returns(new Item(new ItemInfo(_universe.BluePrint.Code)));

			var item = BluePrint.Build(itemFactory.Object);
			Assert.That(item.ItemInfo.Code, Is.EqualTo(_universe.BluePrint.Code));
			Assert.That(item.Quantity, Is.EqualTo(_universe.BluePrint.TargetQuantity));
		}
	}
}