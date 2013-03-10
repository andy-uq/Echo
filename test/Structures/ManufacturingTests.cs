using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Items;
using Echo.State;
using Echo.Tasks.Structure;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.Structures
{
	[TestFixture]
	public class ManufacturingTests
	{
		private readonly MockUniverse _universe = new MockUniverse();
		private Mock<IItemFactory> _itemFactory;

		[SetUp]
		public void CreateItemFactory()
		{
			_itemFactory = new Moq.Mock<IItemFactory>(MockBehavior.Strict);
			_itemFactory.Setup(f => f.Build(_universe.BluePrint.Code, 1)).Returns(new Item(new ItemInfo(_universe.BluePrint.Code)));
		}

		private ManufacturingTask CreateManufacturingTask()
		{
			var manufacturing = new ManufacturingTask(_itemFactory.Object);
			return manufacturing;
		}

		[Test]
		public void CreateTask()
		{
			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters();

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result, Is.InstanceOf<ManufacturingResult>());
		}

		[Test]
		public void RequireBlueprint()
		{
			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = null
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingBluePrint));
		}

		[Test]
		public void RequireAgent()
		{
			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = null,
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingAgent));
		}

		[Test]
		public void RequireAgentSkill()
		{
			var agent = _universe.John.StandUp();
			agent.Skills.Clear();

			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingSkillRequirement));
		}

		[Test]
		public void CreateItem()
		{
			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = _universe.John.StandUp(),
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.Success));
			Assert.That(result.Item.ItemInfo.Code, Is.EqualTo(_universe.BluePrint.Code));
			Assert.That(result.Item.Quantity, Is.EqualTo(_universe.BluePrint.TargetQuantity));
		}
	}
}