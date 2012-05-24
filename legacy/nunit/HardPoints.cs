using System;

using Echo.Entities;
using Echo.Objects;
using Echo.Ships;
using Echo.Vectors;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class HardPointTests
	{
		#region Location Mock

		class Location : BaseLocation, ILocation
		{
			private Vector position;

			public Location(double x, double y, double z)
			{
				position = new Vector(x, y, z);
			}

			protected override string SystematicNamePrefix
			{
				get { return "L"; }
			}

			public override ObjectType ObjectType
			{
				get { return ObjectType.Unknown; }
			}

			Vector ILocation.UniversalCoordinates
			{
				get { return position; }
				set { throw new NotImplementedException(); }
			}

			Vector ILocation.LocalCoordinates
			{
				get { return position; }
				set { throw new NotImplementedException(); }
			}
		}

		#endregion

		[Test]
		public void MaxExtents()
		{
			var ship = new Ship(new Corporation());
			var hardPoint = new HardPoint(ship, HardPointPosition.Right);

			var aimAt = new Location(1, 1, 0); // 45* forward
			Assert.IsTrue(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			hardPoint.Reset();

			aimAt = new Location(1, -1, 0); // 45* behind
			Assert.IsTrue(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// now test we can't move further downward
			aimAt = new Location(1, -1.1d, 0);
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);
		}

		[Test]
		public void TestMovesToExtent()
		{
			var ship = new Ship(new Corporation());
			var hardPoint = new HardPoint(ship, HardPointPosition.Right);

			var aimAt = new Location(0, 1, 0); // 90* forward
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// assert we moved 45*
			Assert.AreEqual(Math.Round(Math.PI / 4, 4), Math.Round(Vector.Angle(hardPoint.Origin, hardPoint.Orientation), 4));

			// assert we moved forward
			Assert.Greater(hardPoint.Orientation.Y, 0d);

			// now make sure the hard point knows this =)
			Assert.AreEqual(45d, hardPoint.Inclination);

			hardPoint.Reset();

			aimAt = new Location(0, -1, 0); // 90* back
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// assert we moved 45*
			Assert.AreEqual(Math.Round(Math.PI / 4, 4), Math.Round(Vector.Angle(hardPoint.Origin, hardPoint.Orientation), 4));

			// assert we moved back
			Assert.Less(hardPoint.Orientation.Y, 0d);

			// now make sure the hard point knows this =)
			Assert.AreEqual(-45d, hardPoint.Inclination);
		}

		[Test]
		public void TestMovesMaxPossible()
		{
			var ship = new Ship(new Corporation());
			var hardPoint = new HardPoint(ship, HardPointPosition.Right);

			var aimAt = new Location(1, -1, 0); // 45* back
			Assert.IsTrue(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// assert we moved 45*
			Assert.AreEqual(Math.Round(Math.PI / 4, 4), Math.Round(Vector.Angle(hardPoint.Origin, hardPoint.Orientation), 4));


			// assert we moved back
			Assert.Less(hardPoint.Orientation.Y, 0d);

			aimAt = new Location(1, 1, 0); // 45* forward
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// assert we moved back to origin
			Assert.AreEqual(hardPoint.Origin, hardPoint.Orientation);

			aimAt = new Location(1, 1, 0); // 45* forward
			Assert.IsTrue(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// assert we moved 45*
			Assert.AreEqual(Math.Round(Math.PI / 4, 4), Math.Round(Vector.Angle(hardPoint.Origin, hardPoint.Orientation), 4));

			// assert we moved back
			Assert.Greater(hardPoint.Orientation.Y, 0d);

			aimAt = new Location(1, -1, 0); // 45* back
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			// assert we moved back to origin
			Assert.AreEqual(hardPoint.Origin, hardPoint.Orientation);
		}

		[Test]
		public void Extremes()
		{
			var ship = new Ship(new Corporation());
			var hardPoint = new HardPoint(ship, HardPointPosition.Right);

			var aimAt = new Location(-1, 1, 0); // 135* forward
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);

			hardPoint = new HardPoint(ship, HardPointPosition.Front) { Speed = 1d };

			aimAt = new Location(0, -1, 0); // 180* backward
			Assert.IsFalse(hardPoint.AimAt(aimAt), "Position: {0:n2}", hardPoint.Inclination);
		}

		[Test]
		public void TrackAnotherShip()
		{
			const double toDegrees = (0.5/Math.PI)*360d;

			var universe = new Universe();
			var ship = new Ship(new Corporation() { Location = universe }.Recruit()) { Name="GG", Speed = 0.5d, Location = universe };
			var track = new Ship(new Corporation() { Location = universe }.Recruit()) { Name="BG", Speed = 100d, UniversalCoordinates = new Vector(100, -1000, 0), Location = universe };

			var hardPoint = new HardPoint(ship, HardPointPosition.Right);

			ship.Destination = new Vector(0, 10, 0);
			track.Destination = new Vector(100, 1000, 0);

			ulong tick = 0;
			while (ship.HasDestination)
			{
				ship.Tick(tick);
				track.Tick(tick);

				Vector target = (track.UniversalCoordinates - ship.UniversalCoordinates);
				Console.WriteLine("Distance to target: {0:n2}, Hardpoint angle to target: {1:n2}* (delta: {2:n2}*)", target.Magnitude, Vector.Angle(hardPoint.Origin, target) * toDegrees, Vector.Angle(hardPoint.Orientation, target) * toDegrees);

				bool inRange = hardPoint.InRange(track);
				bool canTrack = hardPoint.CanTrack(track);

				bool isTracking = hardPoint.AimAt(track);
				Console.WriteLine("{0} {1} target {2}", ship.Name, isTracking ? "has" : "does not have", (inRange) ? (canTrack ? hardPoint.Inclination.ToString("n2")+"*" : "[Out of position]") : "[Out of range]");

				tick++;
			}
		}
	}
}