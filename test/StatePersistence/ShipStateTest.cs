using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class ShipStateTest : StateTest
	{
		public ShipState Ship { get { return Universe.Ship; }}
		
		private Ship Build(ShipState state)
		{
			var builder = Echo.Ships.Ship.Builder.Build(null, state);
			builder
				.Dependent(new ShipInfo { Code = state.Code })
				.Build(info => new ObjectBuilder<ShipInfo>(info));

			return builder.Materialise();
		}

		[Test]
		public void Serialise()
		{
			Database.UseOnceTo().Insert(Ship);
			DumpObjects("Ship");
		}

		[Test]
		public void Save()
		{
			var ship = Build(Ship);
			Assert.That(ship, Is.InstanceOf<Ship>());

			var state = Echo.Ships.Ship.Builder.Save(ship);

			Assert.That(state.Pilot, Is.Not.Null);

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

			var builder = SolarSystem.Builder.Build(null, Universe.SolarSystem);
			builder.Dependent(new ShipInfo {Code = Ship.Code}).Build(x => new ObjectBuilder<ShipInfo>(x));
			
			var solarSystem = builder.Materialise();
			var shipBuilder = Echo.Ships.Ship.Builder.Build(solarSystem, state);
			shipBuilder.Dependent(new ShipInfo {Code = Ship.Code}).Build(x => new ObjectBuilder<ShipInfo>(x));
			var ship = shipBuilder.Materialise();

			Assert.That(ship.Position.LocalCoordinates, Is.EqualTo(Ship.LocalCoordinates));
			Assert.That(ship.SolarSystem, Is.EqualTo(solarSystem));
			Assert.That(ship.Position.GetSolarSystem(), Is.EqualTo(solarSystem));
		}
	}
}