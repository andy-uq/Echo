﻿using System;
using System.Linq;
using Echo.Builder;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class ShipTests
	{
		[Test]
		public void ShipWithNoHardPointsCantTrack()
		{
			var ship = Build(new ShipState { Code = ItemCode.LightFrigate, LocalCoordinates = new Vector(0, 0, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });
			var ship2 = Build(new ShipState { LocalCoordinates = new Vector(0, 0, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });
			
			Assert.That(ship.HardPoints, Is.Empty);
			Assert.That(ship.CanAimAt(ship2), Is.False);
			Assert.That(ship.CanTrack(ship2), Is.False);
		}

		[Test]
		public void ShipCanTrack()
		{

			HardPoint.CalculateHardPoint(HardPointPosition.Left, out var left, out var radiansOfMovement);
			HardPoint.CalculateHardPoint(HardPointPosition.Right, out var right, out radiansOfMovement);
			
			Assert.That(left, Is.Not.EqualTo(Vector.Zero));
			Assert.That(right, Is.Not.EqualTo(Vector.Zero));

			var state = new ShipState
			{
				HardPoints = new[]
				{
					new HardPointState {Position = HardPointPosition.Left, Speed = 0.5d, Orientation = left },
					new HardPointState {Position = HardPointPosition.Right, Speed = 0.5d, Orientation = right }
				},
				LocalCoordinates = new Vector(0, 0, 0),
				Statistics = Enumerable.Empty<ShipStatisticState>()
			};

			var ship = Build(state);
			var inFront = Build(new ShipState { LocalCoordinates = new Vector(0, 10, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });
			var toTheSide = Build(new ShipState { LocalCoordinates = new Vector(-10, 0, 0), Statistics = Enumerable.Empty<ShipStatisticState>() });

			ship.ShouldSatisfyAllConditions
				(
					() => ship.HardPoints.ShouldNotBeEmpty(),
					() => ship.CanAimAt(toTheSide).ShouldBe(true),
					() => ship.CanTrack(toTheSide).ShouldBe(true),
					() => ship.CanAimAt(inFront).ShouldBe(false),
					() => ship.CanTrack(inFront).ShouldBe(false)
				);

			Should.Throw<ArgumentException>(() => ship.CanAimAt(ship)).Message.ShouldBe("Target must not be at the same position as the ship");
			Should.Throw<ArgumentException>(() => ship.CanTrack(ship)).Message.ShouldBe("Target must not be at the same position as the ship");
		}

		private Ship Build(ShipState state)
		{
			state.Code = ItemCode.LightFrigate;
			var builder = Ship.Builder.Build(null, state);
			builder
				.Dependent(new ShipInfo {Code = ItemCode.LightFrigate})
				.Build(info => new ObjectBuilder<ShipInfo>(info));

			return builder.Materialise();
		}
	}
}