using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Builders;
using Echo.Celestial;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class ShipStateTest : StateTest
	{
		public ShipState Ship { get { return Universe.Ship; }}

		[Test]
		public void Serialise()
		{
			Database.UseOnceTo().Insert(Ship);
			DumpObjects("Ship");
		}

		[Test]
		public void Save()
		{
			var ship = Ship.Build(null);
			Assert.That(ship, Is.InstanceOf<Ship>());

			var state = Echo.Ships.Ship.Builder.Save(ship);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Ship);
			var state = Database.UseOnceTo().GetById<ShipState>(Ship.Id);
			Assert.That(state, Is.Not.Null);

			Assert.That(state.Name, Is.EqualTo(Ship.Name));
			Assert.That(state.LocalCoordinates, Is.EqualTo(Ship.LocalCoordinates));
			Assert.That(state.HardPoints, Has.Some.Matches<HardPointState>(x => x.Position == HardPointPosition.Front));
			Assert.That(state.HardPoints.First().Orientation, Is.EqualTo(HardPoint.CalculateOrientation(HardPointPosition.Front)));

			var solarSystem = SolarSystem.Builder.Build(null, Universe.SolarSystem);
			var ship = Echo.Ships.Ship.Builder.Build(solarSystem, state);

			Assert.That(ship.SolarSystem, Is.EqualTo(solarSystem));
			Assert.That(ship.Position.GetSolarSystem(), Is.EqualTo(solarSystem));
		}
	}
}