using Echo.Ships;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class HardPointTests
	{
		private readonly Ship _leftTarget = new Ship { Position = new Position(new Universe(), new Vector(-1, 0, 0)) };
		private readonly Ship _rightTarget = new Ship { Position = new Position(new Universe(), new Vector(1, 0, 0)) };
		private readonly Ship _frontTarget = new Ship { Position = new Position(new Universe(), new Vector(0, 1, 0)) };
		private readonly Ship _rearTarget = new Ship { Position = new Position(new Universe(), new Vector(0, -1, 0)) };
		private readonly Ship _sideTarget = new Ship { Position = new Position(new Universe(), new Vector(-1, 1, 0)) };
		private Ship _ship;

		private class CanTrackResult
		{
			public bool Left { get; set; }
			public bool Right { get; set; }
			public bool Front { get; set; }
			public bool Rear { get; set; }
			public bool LeftTop { get; set; }
		}

		[SetUp]
		public void SetUpShip()
		{
			_ship = new Ship();
		}

		[TestCase(HardPointPosition.Left, "0,1", "-1,0")]
		[TestCase(HardPointPosition.Left, "1,0", "0,1")]
		[TestCase(HardPointPosition.Left, "0,-1", "1,0")]
		[TestCase(HardPointPosition.Left, "-1,0", "0,-1")]

		[TestCase(HardPointPosition.Right, "0,1", "1,0")]
		[TestCase(HardPointPosition.Right, "1,0", "0,-1")]
		[TestCase(HardPointPosition.Right, "0,-1", "-1,0")]
		[TestCase(HardPointPosition.Right, "-1,0", "0,1")]
		public void RotateHardPointToHeading(HardPointPosition hardPointPosition, string heading, string expected)
		{
			var hp = HardPoint.CalculateOrientation(hardPointPosition);

			var headingVector = Vector.Parse(heading);
			var upVector = new Vector(0, 1);

			var rightVector = (headingVector * upVector);
			
			var rotate = Vector.Angle(upVector, headingVector);
			var newHp = rightVector.Z >= 0d ? hp.RotateZ(-rotate) : hp.RotateZ(rotate);
			
			Assert.That(newHp, Is.EqualTo(Vector.Parse(expected)));
		}

		[Test]
		public void CanTrack()
		{
			CanTrack(HardPointPosition.Left, new CanTrackResult { Left = true, LeftTop = true });
			CanTrack(HardPointPosition.Right, new CanTrackResult { Right = true, LeftTop = false });
			CanTrack(HardPointPosition.Rear, new CanTrackResult { Left = true, Right = true, Rear = true, LeftTop = false });
			CanTrack(HardPointPosition.Front, new CanTrackResult { Left = true, Right = true, Front = true, LeftTop = true });
			CanTrack(HardPointPosition.Top, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(HardPointPosition.Bottom, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
		}

		[Test]
		public void NotFastEnough()
		{
			const double pi = System.Math.PI;
			const double tolerance = 0.01;
		
			var hp = new HardPoint(_ship, HardPointPosition.Left) { Speed = 0.25 };
			
			var pointDown = hp.Origin.RotateZ(pi/4);

			hp.Inclination.ShouldBe(0, tolerance);
			
			hp.AimAt(pointDown).ShouldBe(false);
			hp.Inclination.ShouldBe(45.0/2, tolerance);

			hp.AimAt(pointDown).ShouldBe(true);
			hp.Inclination.ShouldBe(45.0, tolerance);

			var nowPointUp = hp.Origin.RotateZ(-pi/4);

			hp.AimAt(nowPointUp).ShouldBe(false);
			hp.Inclination.ShouldBe(45.0/2, tolerance);

			hp.AimAt(nowPointUp);//.ShouldBe(false);
			hp.Inclination.ShouldBe(0, tolerance);
		}

		[Test]
		public void OutOfRange()
		{
			const double pi = System.Math.PI;
			const double tolerance = 0.01;
		
			var hp = new HardPoint(_ship, HardPointPosition.Left) { Speed = 0.25 };
			
			var pointDown = hp.Origin.RotateZ(pi);

			hp.Inclination.ShouldBe(0, tolerance);
			
			hp.AimAt(pointDown).ShouldBe(false);
			hp.Inclination.ShouldBe(45.0/2, tolerance);

			hp.AimAt(pointDown).ShouldBe(true);
			hp.Inclination.ShouldBe(45.0, tolerance);

			// stop moving down -- limit of range
			hp.AimAt(pointDown).ShouldBe(false);
			hp.Inclination.ShouldBe(45.0, tolerance);
		}

		[Test]
		public void CanTrackWithShipHeading()
		{
			var headingRight = new Vector(1,0);

			CanTrack(headingRight, HardPointPosition.Left, new CanTrackResult { Front = true, LeftTop = true });
			CanTrack(headingRight, HardPointPosition.Right, new CanTrackResult { Rear = true });
			CanTrack(headingRight, HardPointPosition.Top, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(headingRight, HardPointPosition.Bottom, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(headingRight, HardPointPosition.Front, new CanTrackResult { Right = true, Rear = true, Front = true });
			CanTrack(headingRight, HardPointPosition.Rear, new CanTrackResult { Front = true, Left = true, LeftTop = true, Rear = true });

			var headingLeft = new Vector(-1,0);

			CanTrack(headingLeft, HardPointPosition.Left, new CanTrackResult { Rear = true });
			CanTrack(headingLeft, HardPointPosition.Right, new CanTrackResult { Front = true, LeftTop = true });
			CanTrack(headingLeft, HardPointPosition.Top, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(headingLeft, HardPointPosition.Bottom, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(headingLeft, HardPointPosition.Front, new CanTrackResult { Front = true, Left = true, LeftTop = true, Rear = true });
			CanTrack(headingLeft, HardPointPosition.Rear, new CanTrackResult { Right = true, Rear = true, Front = true });

			var headingDown = new Vector(0,-1);

			CanTrack(headingDown, HardPointPosition.Left, new CanTrackResult { Right = true });
			CanTrack(headingDown, HardPointPosition.Right, new CanTrackResult { Left = true, LeftTop = true });
			CanTrack(headingDown, HardPointPosition.Top, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(headingDown, HardPointPosition.Bottom, new CanTrackResult { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
			CanTrack(headingDown, HardPointPosition.Front, new CanTrackResult { Rear = true, Left = true, Right = true });
			CanTrack(headingDown, HardPointPosition.Rear, new CanTrackResult { Right = true, Front = true, LeftTop = true, Left = true });
		}

		[Test]
		public void Aims()
		{
			var hp = new HardPoint(_ship, HardPointPosition.Top);
			hp.AimAt(_leftTarget);
			Assert.That(hp.Inclination, Is.EqualTo(90d));

			hp.AimAt(_rightTarget);
			Assert.That(hp.Inclination, Is.EqualTo(90d));

			hp.AimAt(_frontTarget);
			Assert.That(hp.Inclination, Is.EqualTo(0d));

			hp.AimAt(_rearTarget);
			Assert.That(hp.Inclination, Is.EqualTo(180d));
		}

		private void CanTrack(HardPointPosition position, CanTrackResult expected)
		{
			var hp = new HardPoint(_ship, position);
			Assert.That(hp.CanTrack(_leftTarget), Is.EqualTo(expected.Left), "Can track left");
			Assert.That(hp.CanTrack(_rightTarget), Is.EqualTo(expected.Right), "Can track right");
			Assert.That(hp.CanTrack(_frontTarget), Is.EqualTo(expected.Front), "Can track front");
			Assert.That(hp.CanTrack(_rearTarget), Is.EqualTo(expected.Rear), "Can track rear");
			Assert.That(hp.CanTrack(_sideTarget), Is.EqualTo(expected.LeftTop), "Can track left-top");
		}

		private void CanTrack(Vector shipHeading, HardPointPosition position, CanTrackResult expected)
		{
			_ship.Heading = shipHeading;
			CanTrack(position, expected);
		}
	}
}