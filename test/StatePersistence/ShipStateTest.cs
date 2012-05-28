using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class ShipStateTest : StateTest
	{
		private ShipState _shipState;

		[SetUp]
		public void SetUpShipState()
		{
			_shipState = new ShipState()
			{
				Id = 1,
				Name = "Ship-1",
				LocalCoordinates = new Vector(10, 10, 0),
				HardPoints = new[] { new HardPointState { Position = HardPointPosition.Front, Orientation = new Vector(0, -1, 0) } }
			};
		}

		[Test]
		public void Serialise()
		{
			Database.UseOnceTo().Insert(_shipState);
			DumpObjects("Ship");
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(_shipState);
			var state = Database.UseOnceTo().GetById<ShipState>(1L);
			Assert.That(state, Is.Not.Null);

			Assert.That(state.Name, Is.EqualTo(_shipState.Name));
			Assert.That(state.LocalCoordinates, Is.EqualTo(_shipState.LocalCoordinates));
			Assert.That(state.HardPoints, Has.Some.Matches<HardPointState>(x => x.Position == HardPointPosition.Front));
			Assert.That(state.HardPoints.First().Orientation, Is.EqualTo(new Vector(0, -1, 0)));

			var solarSystem = SolarSystem.Builder.Build(null, Universe.SolarSystem);
			var ship = Ship.Builder.Build(solarSystem, state);

			Assert.That(ship.SolarSystem, Is.EqualTo(solarSystem));
			Assert.That(ship.Position.GetSolarSystem(), Is.EqualTo(solarSystem));
		}
	}
}