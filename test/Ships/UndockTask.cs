using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Celestial;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using Echo.Tasks;
using Echo.Tasks.Ships;
using Echo.Tasks.Ships.Undocking;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;
using SkillLevel = Echo.Agents.Skills.SkillLevel;
using SkillTraining = Echo.Agents.Skills.SkillTraining;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class UndockTaskTests
	{
		private MockUniverse _universe;
		private UndockShipTask _task;
		
		[SetUp]
		public void SetUp()
		{
			_universe = new MockUniverse();

			var mock = new Mock<ILocationServices>();
			mock.Setup(x => x.GetExitPosition(It.IsAny<ILocation>())).Returns<ILocation>(l => l.Position.LocalCoordinates);
			
			_task = new UndockShipTask(mock.Object);
		}

		[Test]
		public void CantUndockWhenNotDocked()
		{
			var ship = new Ship();
			var pilot = new Agent();

			_task.SetParameters(new UndockShipParameters(ship, pilot));

			var result = (UndockShipResult )_task.Execute();
			Assert.That(result.Success, Is.False);
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.NotDocked));

			ITaskResult taskResult = result;
			Assert.That(taskResult.StatusCode, Is.EqualTo("NotDocked"));
			Assert.That(taskResult.ErrorParams, Has.Property("Ship").EqualTo(ship));
			Assert.That(taskResult.ErrorParams, Has.Property("Pilot").Null);
		}

		[Test]
		public void CantUndockWithoutShipSkill()
		{
			var structure = new Manufactory();
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			
			var pilot = _universe.John.StandUp();
			pilot.Skills[SkillCode.SpaceshipCommand].Level = 0;

			_task.SetParameters(new UndockShipParameters(ship, pilot));
			var result = (UndockShipResult)_task.Execute();
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.MissingSkillRequirement));
		}

		[Test]
		public void CantUndockWithoutShipSkillLevel()
		{
			var structure = new Manufactory();
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent { Skills = { { new SkillLevel(_universe.SpaceshipCommand, level: 1) } } };

			_task.SetParameters(new UndockShipParameters(ship, pilot));
			var result = (UndockShipResult)_task.Execute();
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.MissingSkillRequirement));
		}

		[Test]
		public void CanUndockWithShipSkill()
		{
			var solarSystem = new SolarSystem();
			var structure = new Manufactory { Position = new Position(solarSystem, Vector.Parse("0,1,0")) };
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent { Skills = { { new SkillLevel(_universe.SpaceshipCommand, level: 5) } } };

			_task.SetParameters(new UndockShipParameters(ship, pilot));
			
			var result = _task.Execute();
			Assert.That(result.Success, Is.True, result.StatusCode);
			Assert.That(ship.Position.LocalCoordinates, Is.EqualTo(structure.Position.LocalCoordinates));
			Assert.That(ship.Position.Location, Is.EqualTo(solarSystem));
		}

		private ShipInfo GetShipInfo()
		{
			return new ShipInfo
			{
				PilotRequirements = new[]
				{
					new State.SkillLevel(SkillCode.SpaceshipCommand, level:5)
				}
			};
		}
	}	
}