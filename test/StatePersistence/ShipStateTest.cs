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

			builder
				.Dependent(Universe.Weapon)
				.Build(x => new ObjectBuilder<WeaponInfo>(x));

			return builder.Materialise();
		}

		[Test]
		public void Serialise()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(Ship);
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var ship = Build(Ship);
			Assert.That(ship, Is.InstanceOf<Ship>());

			var state = Echo.Ships.Ship.Builder.Save(ship);

			Assert.That(state.Pilot, Is.Not.Null);

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			using ( var session = Database.OpenSession() )
			{
				session.Store(Ship, string.Concat("Ship/", Ship.ObjectId));
				session.SaveChanges();
			}

			using ( var session = Database.OpenSession() )
			{
				var state = session.Load<ShipState>(Ship.Id);
				Assert.That(state, Is.Not.Null);

				Assert.That(state.Name, Is.EqualTo(Ship.Name));
				Assert.That(state.LocalCoordinates, Is.EqualTo(Ship.LocalCoordinates));
				Assert.That(state.HardPoints, Has.Some.Matches<HardPointState>(x => x.Position == HardPointPosition.Front));
				Assert.That(state.HardPoints.First().Orientation, Is.EqualTo(HardPoint.CalculateOrientation(HardPointPosition.Front)));

				var builder = SolarSystem.Builder.Build(null, Universe.SolarSystem);
				builder.Dependent(new ShipInfo {Code = Ship.Code}).Build(x => new ObjectBuilder<ShipInfo>(x));
				builder.Dependent(Universe.Weapon).Build(x => new ObjectBuilder<WeaponInfo>(x));

				var solarSystem = builder.Materialise();
				var shipBuilder = Echo.Ships.Ship.Builder.Build(solarSystem, state);
				shipBuilder.Dependent(new ShipInfo {Code = Ship.Code}).Build(x => new ObjectBuilder<ShipInfo>(x));
				shipBuilder.Dependent(Universe.Weapon).Build(x => new ObjectBuilder<WeaponInfo>(x));
				var ship = shipBuilder.Materialise();

				CheckPosition(solarSystem, ship);
				CheckWeaponState(ship);
			}
		}

		private void CheckWeaponState(Ship ship)
		{
			Assert.That(ship.HardPoints, Is.Not.Empty);
			var hp = ship.HardPoints.First();

		}

		private void CheckPosition(SolarSystem solarSystem, Ship ship)
		{
			Assert.That(ship.Position.LocalCoordinates, Is.EqualTo(Ship.LocalCoordinates));
			Assert.That(ship.SolarSystem, Is.EqualTo(solarSystem));
			Assert.That(ship.Position.GetSolarSystem(), Is.EqualTo(solarSystem));
		}
	}
}