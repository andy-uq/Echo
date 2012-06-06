using System.Linq;
using Echo.Builders;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class ShipTests
	{
		[Test]
		public void ShipWithNoHardPointsCantTrack()
		{
			var ship = Build(new ShipState { LocalCoordinates = new Vector(0, 0, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });
			var ship2 = Build(new ShipState { LocalCoordinates = new Vector(0, 0, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });
			
			Assert.That(ship.HardPoints, Is.Empty);
			Assert.That(ship.CanAimAt(ship2), Is.False);
			Assert.That(ship.CanTrack(ship2), Is.False);
		}

		[Test]
		public void ShipCanTrack()
		{
			Vector left, right;
			double radiansOfMovement;

			HardPoint.CalculateHardPoint(HardPointPosition.Left, out left, out radiansOfMovement);
			HardPoint.CalculateHardPoint(HardPointPosition.Right, out right, out radiansOfMovement);
			
			Assert.That(left, Is.Not.EqualTo(Vector.Zero));
			Assert.That(right, Is.Not.EqualTo(Vector.Zero));

			var state = new ShipState
			{
				HardPoints = new[]
				{
					new HardPointState {Position = HardPointPosition.Left, Speed = 0.5d, Orientation = left },
					new HardPointState {Position = HardPointPosition.Right, Speed = 0.5d, Orientation = right },
				},
				LocalCoordinates = new Vector(0, 0, 0),
				Statistics = Enumerable.Empty<ShipStatisticState>()
			};

			var ship = Build(state);
			var inFront = Build(new ShipState { LocalCoordinates = new Vector(0, 10, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });
			var toTheSide = Build(new ShipState { LocalCoordinates = new Vector(-10, 0, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });

			Assert.That(ship.HardPoints, Is.Not.Empty);

			Assert.That(ship.CanAimAt(toTheSide), Is.True);
			Assert.That(ship.CanTrack(toTheSide), Is.True);

			Assert.That(ship.CanAimAt(inFront), Is.False);
			Assert.That(ship.CanTrack(inFront), Is.False);
		}

		private Ship Build(ShipState state)
		{
			return Ship.Builder.Build(null, state).Materialise();
		}
	}
}