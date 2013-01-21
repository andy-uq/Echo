using System.ComponentModel;
using System.Linq;
using Echo.Celestial;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using Echo.Tests.Mocks;
using NUnit.Framework;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class MiningTests
	{
		private Ship _ship;
		private SolarSystem _solarSystem;
		private AsteroidBelt _asteroidBelt;

		[SetUp]
		public void SetUp()
		{
			_solarSystem = new SolarSystem();
			_asteroidBelt = new AsteroidBelt { Richness = 500, AmountRemaining = 1000 };
			_solarSystem.Satellites.Add(_asteroidBelt);

			var shipInfo = new ShipState(ItemCode.LightFrigate)
			{
				HardPoints = new[] { new HardPointState { Position = HardPointPosition.Bottom, Weapon = new WeaponState { Code = ItemCode.MiningLaser } }  }
			};

			var builder = Ship.Builder.Build(_solarSystem, shipInfo);
			var frigate = new ShipInfo { Code = ItemCode.LightFrigate };
			var miningLaser = new WeaponInfo { Code = ItemCode.MiningLaser, DamageType = DamageType.Energy, MinimumDamage = 5, MaximumDamage = 5, Speed = 1d };

			IIdResolver resolver = new IdResolutionContext(new IObject[] { miningLaser, frigate });
			_ship = builder.Build(resolver);
		}

		[Test]
		public void CreateShip()
		{
			var mining = new MiningTask(new LocationService()) { };
			var result = mining.Mine(_ship, _asteroidBelt);

			Assert.That(result.Success, Is.True);
		}
	}
}