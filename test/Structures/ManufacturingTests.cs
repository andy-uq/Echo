using System.Linq;
using Echo.Agents.Skills;
using Echo.Builder;
using Echo.Corporations;
using Echo.Items;
using Echo.State;
using Echo.Structures;
using Echo.Tasks.Structure;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;
using test.common;

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

			SetupItem(_universe.BluePrint);
			SetupItem(_universe.ShipBluePrint);

			var builder = Echo.Structures.Manufactory.Builder.For(_universe.Manufactory).Build(null);
			builder.Add(Corporation.Builder.Build(_universe.MSCorp));
			
			_manufactory = builder.Materialise();
		}

		private void SetupItem(BluePrintInfo bluePrint)
		{
			var item = TestItems.BuildItem(bluePrint.Code, bluePrint.TargetQuantity);
			_itemFactory.Setup(f => f.Build(bluePrint.Code, bluePrint.TargetQuantity)).Returns(item);
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
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.MissingBluePrint));
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
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.MissingAgent));
		}

		[Test]
		public void RequireAgentSkill()
		{
			var agent = _universe.John.StandUp();
			agent.Skills[SkillCode.SpaceshipCommand].Level = 0;

			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};
			
			var manufacturing = CreateManufacturingTask(parameters);
			var result = manufacturing.Manufacture();
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.MissingSkillRequirement));
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
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.MissingAgent));
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
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.MissingMaterials));
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

			Assert.That(result.Success, Is.True);
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.Success));

			var item = result.Item;
			Assert.That(item, Is.Not.Null);
			Assert.That(item.ItemInfo.Code, Is.EqualTo(_universe.BluePrint.Code));
			Assert.That(item.Quantity, Is.EqualTo(_universe.BluePrint.TargetQuantity));

			Assert.That(materials.Quantity, Is.EqualTo(10));
		}

		[Test]
		public void BuildMaterialsCalculatedCorrectly()
		{
			Corporation corporation = Corporation.Builder.Build(_universe.MSCorp).Materialise();

			var property = corporation.GetProperty(Manufactory);
			property.Add(TestItems.BuildItem(ItemCode.Veldnium, quantity: 2000));
			property.Add(TestItems.BuildItem(ItemCode.MissileLauncher, quantity: 20));
			property.Add(TestItems.BuildItem(ItemCode.MiningLaser, quantity: 20));
			property.Add(TestItems.BuildItem(ItemCode.EnergyShield, quantity: 20));
			
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.ShipBluePrint,
				Agent = _universe.John.StandUp(corporation, initialLocation: Manufactory),
			};

			var manufacturing = CreateManufacturingTask(parameters);
			
			foreach ( var item in manufacturing.BluePrint.Materials )
			{
				var firstLoad = manufacturing.FirstLoad.Single(x => x.Code == item.Code);
				var subsequentLoad = manufacturing.SubsequentLoad.Single(x => x.Code == item.Code);

				Assert.That(firstLoad.Quantity + (subsequentLoad.Quantity * (parameters.BluePrint.BuildLength - 1)), Is.EqualTo(item.Quantity), item.Code.ToString());
			}
		}

		[Test]
		public void CreateItemOverTime()
		{
			Corporation corporation = Corporation.Builder.Build(_universe.MSCorp).Materialise();
	
			var property = corporation.GetProperty(Manufactory);
			property.Add(TestItems.BuildItem(ItemCode.Veldnium, quantity: 2000));
			property.Add(TestItems.BuildItem(ItemCode.MissileLauncher, quantity: 20));
			property.Add(TestItems.BuildItem(ItemCode.MiningLaser, quantity: 20));
			property.Add(TestItems.BuildItem(ItemCode.EnergyShield, quantity: 20));
			
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.ShipBluePrint,
				Agent = _universe.John.StandUp(corporation, initialLocation:Manufactory),
			};
			
			var manufacturing = CreateManufacturingTask(parameters);

			Assert.That(manufacturing.TimeRemaining, Is.EqualTo(_universe.ShipBluePrint.BuildLength));

			ManufacturingResult result;
			var buildLength = 5;
			var quanta = 200;
			while ( buildLength > 1 )
			{
				result = manufacturing.Manufacture();

				var veldnium = property.Single(v => v.ItemInfo.Code == ItemCode.Veldnium);

				Assert.That(result.Success, Is.True);
				Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.Pending));

				buildLength--;
				Assert.That(veldnium.Quantity, Is.EqualTo(1000 + (buildLength * quanta)));
			}

			result = manufacturing.Manufacture();

			Assert.That(result.Success, Is.True);
			Assert.That(result.StatusCode, Is.EqualTo(ManufacturingTask.StatusCode.Success));
			Assert.That(result.Item, Is.Not.Null);
			Assert.That(result.Item.ItemInfo.Code, Is.EqualTo(_universe.ShipBluePrint.Code));
			Assert.That(result.Item.Quantity, Is.EqualTo(_universe.ShipBluePrint.TargetQuantity));
		}
	}
}