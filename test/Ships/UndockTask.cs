using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Celestial;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using Moq;
using NUnit.Framework;
using SkillLevel = Echo.State.SkillLevel;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class UndockTaskTests
	{
		private UndockShipTask _task;
		
		[SetUp]
		public void SetUp()
		{
			var mock = new Moq.Mock<ILocationServices>();
			mock.Setup(x => x.GetExitPosition(It.IsAny<ILocation>())).Returns<ILocation>(l => l.Position.LocalCoordinates);
			
			_task = new UndockShipTask(mock.Object);
		}

		[Test]
		public void CantUndockWhenNotDocked()
		{
			var ship = new Ship();
			var pilot = new Agent();

			var result = _task.Execute(ship, pilot);
			Assert.That(result.Success, Is.False);
			Assert.That(result.ErrorCode, Is.EqualTo(ShipTask.ErrorCode.NotDocked));

			ITaskResult taskResult = result;
			Assert.That(taskResult.ErrorCode, Is.EqualTo("NotDocked"));
			Assert.That(taskResult.ErrorParams, Has.Property("Ship").EqualTo(ship));
			Assert.That(taskResult.ErrorParams, Has.Property("Pilot").Null);
		}

		[Test]
		public void CantUndockWithoutShipSkill()
		{
			var structure = new Manufactory();
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent();

			var result = _task.Execute(ship, pilot);
			Assert.That(result.ErrorCode, Is.EqualTo(ShipTask.ErrorCode.MissingSkillRequirement));
		}

		[Test]
		public void CantUndockWithoutShipSkillLevel()
		{
			var structure = new Manufactory();
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent { Skills = { { SkillCode.SpaceshipCommand, new Agents.SkillLevel { Level = 1 } } } };

			var result = _task.Execute(ship, pilot);
			Assert.That(result.ErrorCode, Is.EqualTo(ShipTask.ErrorCode.MissingSkillRequirement));
		}

		[Test]
		public void CanUndockWithShipSkill()
		{
			var solarSystem = new SolarSystem();
			var structure = new Manufactory { Position = new Position(solarSystem, Vector.Parse("0,1,0")) };
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent { Skills = { { SkillCode.SpaceshipCommand, new Agents.SkillLevel { Level = 5 }  } }};

			var result = _task.Execute(ship, pilot);
			Assert.That(result.Success, Is.True, result.ErrorCode.ToString());
			Assert.That(ship.Position.LocalCoordinates, Is.EqualTo(structure.Position.LocalCoordinates));
			Assert.That(ship.Position.Location, Is.EqualTo(solarSystem));
		}

		private ShipInfo GetShipInfo()
		{
			return new ShipInfo
			{
				PilotRequirements = new[]
				{
					new SkillLevel { Level = 5, SkillCode = SkillCode.SpaceshipCommand }
				}
			};
		}
	}	
}