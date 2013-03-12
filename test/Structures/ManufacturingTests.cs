using Echo.Builder;
using Echo.Corporations;
using Echo.Items;
using Echo.State;
using Echo.Structures;
using Echo.Tasks.Structure;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.Structures
{
	[TestFixture]
	public class ManufacturingTests
	{
		private MockUniverse _universe;
		private Mock<IItemFactory> _itemFactory;
		private Structure _manufactory;

		public Structure Manufactory
		{
			get { return _manufactory; }
		}

		[SetUp]
		public void CreateItemFactory()
		{
			_universe = new MockUniverse();

			_itemFactory = new Moq.Mock<IItemFactory>(MockBehavior.Strict);
			_itemFactory.Setup(f => f.Build(_universe.BluePrint.Code, 1)).Returns(new Item(new ItemInfo(_universe.BluePrint.Code)));

			var builder = Echo.Structures.Manufactory.Builder.For(_universe.Manufactory).Build(null);
			builder.Add(Corporation.Builder.Build(_universe.MSCorp));
			builder.RegisterTestSkills();

			_manufactory = builder.Materialise();
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
		public void RequireAgentAtManufactory()
		{
			var agent = _universe.John.StandUp();

			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingAgent));
		}

		[Test]
		public void CreateItem()
		{
			var manufacturing = CreateManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = _universe.John.StandUp(Manufactory),
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.Success));
			Assert.That(result.Item.ItemInfo.Code, Is.EqualTo(_universe.BluePrint.Code));
			Assert.That(result.Item.Quantity, Is.EqualTo(_universe.BluePrint.TargetQuantity));
		}
	}
}