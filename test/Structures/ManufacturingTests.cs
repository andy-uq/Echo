using Echo.Agents.Skills;
using Echo.Tasks.Structure;
using Echo.Tests.Items;
using Echo.Tests.Mocks;
using NUnit.Framework;

namespace Echo.Tests.Structures
{
	[TestFixture]
	public class ManufacturingTests
	{
		private MockUniverse _universe = new MockUniverse();

		[Test]
		public void CreateTask()
		{
			var manufacturing = new ManufacturingTask();
			var parameters = new ManufacturingParameters();

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result, Is.InstanceOf<ManufacturingResult>());
		}

		[Test]
		public void RequireBlueprint()
		{
			var manufacturing = new ManufacturingTask();
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
			var manufacturing = new ManufacturingTask();
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
			agent.Skills[SkillCode.SpaceshipCommand].Level = 0;

			var manufacturing = new ManufacturingTask();
			var parameters = new ManufacturingParameters
			{
				BluePrint = _universe.BluePrint,
				Agent = agent,
			};

			var result = manufacturing.Manufacture(parameters);
			Assert.That(result.ErrorCode, Is.EqualTo(ManufacturingTask.ErrorCode.MissingSkillRequirement));
		}
	}
}