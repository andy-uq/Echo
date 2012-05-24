using Echo.Ships;
using NUnit.Framework;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class HardPointTests
	{
		private readonly Ship _leftTarget = new Ship() { Position = new Position(new Universe(), new Vector(-1, 0, 0)) };
		private readonly Ship _rightTarget = new Ship() { Position = new Position(new Universe(), new Vector(1, 0, 0)) };
		private readonly Ship _frontTarget = new Ship() { Position = new Position(new Universe(), new Vector(0, 1, 0)) };
		private readonly Ship _rearTarget = new Ship() { Position = new Position(new Universe(), new Vector(0, -1, 0)) };
		private readonly Ship _sideTarget = new Ship() { Position = new Position(new Universe(), new Vector(-1, 1, 0)) };
		private Ship _ship = new Ship();

		private class CanTrackResult
		{
			public bool Left { get; set; }
			public bool Right { get; set; }
			public bool Front { get; set; }
			public bool Rear { get; set; }
			public bool LeftTop { get; set; }
		}

		[Test]
		public void CanTrack()
		{
			CanTrack(HardPointPosition.Left, new CanTrackResult() { Left = true, LeftTop = true });
			CanTrack(HardPointPosition.Right, new CanTrackResult() { Right = true, LeftTop = false });
			CanTrack(HardPointPosition.Rear, new CanTrackResult() { Left = true, Right = true, Rear = true, LeftTop = false });
			CanTrack(HardPointPosition.Front, new CanTrackResult() { Left = true, Right = true, Front = true, LeftTop = true });
			CanTrack(HardPointPosition.Top, new CanTrackResult() { Left = true, Right = true, Front = true, Rear = true, LeftTop = true, });
			CanTrack(HardPointPosition.Bottom, new CanTrackResult() { Left = true, Right = true, Front = true, Rear = true, LeftTop = true });
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
	}
}