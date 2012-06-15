using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Celestial;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;
using SkillLevel = Echo.State.SkillLevel;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class UndockTaskTests
	{
		[Test]
		public void CantUndockWhenNotDocked()
		{
			var ship = new Ship();
			var pilot = new Agent();

			var random = new Moq.Mock<IRandom>();
			var task = new UndockShipTask(new LocationServices(random.Object));
			var result = task.Execute(ship, pilot);
			Assert.That(result.Success, Is.False);
			Assert.That(System.Enum.Parse(typeof(ShipTask.ErrorCode), result.ErrorCode), Is.EqualTo(ShipTask.ErrorCode.NotDocked));
		}

		[Test]
		public void CantUndockWithoutShipSkill()
		{
			var structure = new Manufactory();
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent();

			var random = new Moq.Mock<IRandom>();
			var task = new UndockShipTask(new LocationServices(random.Object));
			var result = task.Execute(ship, pilot);
			Assert.That(System.Enum.Parse(typeof(ShipTask.ErrorCode), result.ErrorCode), Is.EqualTo(ShipTask.ErrorCode.MissingSkillRequirement));
		}

		[Test]
		public void CanUndockWithShipSkill()
		{
			var solarSystem = new SolarSystem();
			var structure = new Manufactory { Position = new Position(solarSystem, Vector.Zero) };
			var ship = new Ship { Position = new Position(structure, Vector.Zero), ShipInfo = GetShipInfo() };
			var pilot = new Agent { Skills = { { SkillCode.SpaceshipCommand, new Agents.SkillLevel { Level = 1 }  } }};

			var random = new Moq.Mock<IRandom>();
			var task = new UndockShipTask(new LocationServices(random.Object));
			var result = task.Execute(ship, pilot);

			Assert.That(result.Success, Is.True, result.ErrorCode);
			Assert.That(ship.Position.Location, Is.EqualTo(solarSystem));
		}

		private ShipInfo GetShipInfo()
		{
			return new ShipInfo
			{
				PilotRequirements = new[]
				{
					new SkillLevel { Level = 1, SkillCode = SkillCode.SpaceshipCommand }
				}
			};
		}
	}	
}