using System.ComponentModel;
using System.Linq;
using Echo.Celestial;
using Echo.Items;
using Echo.Ships;
using Echo.State;
using Echo.Tasks.Ships;
using Echo.Tasks.Ships.Mining;
using Echo.Tests.Mocks;
using Moq;
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
			_asteroidBelt = new AsteroidBelt { Ore = ItemCode.Veldnium, Richness = 500, AmountRemaining = 1000, Position = new Position(_solarSystem, Vector.Parse("1,0")) };
			_solarSystem.Satellites.Add(_asteroidBelt);

			var shipInfo = new ShipState(ItemCode.LightFrigate)
			{
				HardPoints = new[]
				{
					new HardPointState { Position = HardPointPosition.Bottom, Weapon = new WeaponState { Code = ItemCode.MiningLaser }, Speed = 1d },
					new HardPointState { Position = HardPointPosition.Top, Weapon = new WeaponState { Code = ItemCode.MissileLauncher } }
				}
			};

			var builder = Ship.Builder.Build(_solarSystem, shipInfo);
			var frigate = new ShipInfo { Code = ItemCode.LightFrigate };
			var missileLauncher = new WeaponInfo { Code = ItemCode.MissileLauncher, DamageType = DamageType.Thermal, MinimumDamage = 10, MaximumDamage = 100, Speed = 1d };
			var miningLaser = new WeaponInfo { Code = ItemCode.MiningLaser, DamageType = DamageType.Energy, MinimumDamage = 5, MaximumDamage = 5, Speed = 1d };

			IIdResolver resolver = new IdResolutionContext(new IObject[] { missileLauncher, miningLaser, frigate });
			_ship = builder.Build(resolver);
		}

		[Test]
		public void CanMineAsteroid()
		{
			var veldnium = new ItemInfo(ItemCode.Veldnium);
			var itemFactory = new Moq.Mock<IItemFactory>(MockBehavior.Strict);
			itemFactory.Setup(x => x.Build(ItemCode.Veldnium, It.IsAny<uint>())).Returns<ItemCode, uint>((item, quantity) => new Item(veldnium, quantity));

			var mining = new MiningTask(itemFactory.Object) { };
			var result = mining.Execute(new MineAsteroidParameters(_ship, _asteroidBelt));

			Assert.That(result.Success, Is.True);
			Assert.That(result.Ore.Quantity, Is.EqualTo(1));
			Assert.That(_asteroidBelt.AmountRemaining, Is.EqualTo(999));
		}
	}
}