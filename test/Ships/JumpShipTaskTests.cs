using Echo.Agents;
using Echo.Celestial;
using Echo.JumpGates;
using Echo.Ships;
using Echo.Tasks;
using Echo.Tasks.Ships;
using Echo.Tasks.Ships.Jump;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class JumpShipTaskTests
	{
		private JumpShipTask _task;
		
		[SetUp]
		public void SetUp()
		{
			var mock = new Mock<ILocationServices>();
			mock.Setup(x => x.GetExitPosition(It.IsAny<ILocation>())).Returns<ILocation>(l => l.Position.LocalCoordinates);
			
			_task = new JumpShipTask(mock.Object);
		}

		[Test]
		public void CantJumpWithNoPilot()
		{
			var ship = new Ship();
			var jumpGate = new JumpGate();

			_task.SetParameters(new JumpShipParameters(ship, jumpGate));

			var result = (JumpShipResult )_task.Execute();
			Assert.That(result.Success, Is.False);
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.NoPilot));

			ITaskResult taskResult = result;
			Assert.That(taskResult.StatusCode, Is.EqualTo("NoPilot"));
			Assert.That(taskResult.ErrorParams, Has.Property("Ship").EqualTo(ship));
			Assert.That(taskResult.ErrorParams, Has.Property("JumpGate").EqualTo(jumpGate));
			Assert.That(taskResult.ErrorParams, Has.Property("Pilot").Null);
		}

		[Test]
		public void CantJumpWhenNotInRange()
		{
			var position = new Position(new SolarSystem(), Vector.Parse("0,1,0"));
			var jumpGate = new JumpGate { Position = position };
			var ship = new Ship { Position = position + Vector.Parse("1,0,0"), Pilot = new Agent() };

			_task.SetParameters(new JumpShipParameters(ship, jumpGate)); 
			
			var result = (JumpShipResult)_task.Execute();
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.NotInPosition));
		}

		[Test]
		public void CanJumpWhenInRange()
		{
			var position = new Position(new SolarSystem(), Vector.Parse("0,1,0"));
			var ship = new Ship { Position = position, Pilot = new Agent() };

			var target = new JumpGate {Position = new Position(new SolarSystem(), Vector.Parse("-1,1,0"))};
			var jumpGate = new JumpGate { Position = position, ConnectsTo = target };

			_task.SetParameters(new JumpShipParameters(ship, jumpGate));
			
			var result = _task.Execute();
			Assert.That(result.Success, Is.True, result.StatusCode);
			Assert.That(ship.Position.LocalCoordinates, Is.EqualTo(target.Position.LocalCoordinates));
			Assert.That(ship.Position.Location, Is.EqualTo(target.Position.Location));
		}
	}	
}