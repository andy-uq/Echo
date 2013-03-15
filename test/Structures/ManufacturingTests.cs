using System.Linq;
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
			
			_manufactory = builder.Materialise();
		}

		private ManufacturingTask CreateManufacturingTask(ManufacturingParameters parameters)
		{
			var manufacturing = new ManufacturingTask(_itemFactory.Object);
			manufacturing.SetParameters(parameters);

			return manufacturing;
		}

		[Test]
		public void CreateTask()
		{
			var manufacturing = CreateManufacturingTask(new ManufacturingParameters());
			
			var result = manufacturing.Manufacture();
			Assert.That(result, Is.InstanceOf<ManufacturingResult>());
		}

		[Test]
		public void RequireBlueprint()
		{
			var parameters = new ManufacturingParameters
			{
				BluePrint = null
			};

			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingBluePrint));
		}

		[Test]
		public void RequireAgent()
		{
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = null,
			};

			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingAgent));
		}

		[Test]
		public void RequireAgentSkill()
		{
			var agent = _universe.John.StandUp();
			agent.Skills.Clear();

			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};
			
			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingSkillRequirement));
		}

		[Test]
		public void RequireAgentAtManufactory()
		{
			var agent = _universe.John.StandUp();

			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};

			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingAgent));
		}

		[Test]
		public void RequireItemsAtManufactory()
		{
			Corporation corporation = Corporation.Builder.Build(_universe.MSCorp).Materialise();
			var agent = _universe.John.StandUp(corporation, initialLocation:Manufactory);

			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};

			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingMaterials));
		}

		[Test]
		public void CreateItem()
		{
			Corporation corporation = Corporation.Builder.Build(_universe.MSCorp).Materialise();
			var materials = TestItems.BuildItem(ItemCode.Veldnium, quantity: 20);
	
			var property = corporation.GetProperty(Manufactory);
			property.Add(materials);
			
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = _universe.John.StandUp(corporation, initialLocation:Manufactory),
			};
			
			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();

			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.Success));
			Assert.That(result.Item, Is.Not.Null);
			Assert.That(result.Item.ItemInfo.Code, Is.EqualTo(_universe.BluePrint.Code));
			Assert.That(result.Item.Quantity, Is.EqualTo(_universe.BluePrint.TargetQuantity));

			Assert.That(materials.Quantity, Is.EqualTo(10));
		}
	}
}