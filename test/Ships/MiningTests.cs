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
using Shouldly;
using test.common;

namespace Echo.Tests.Ships
{
	[TestFixture]
	public class MiningTests
	{
		private Ship _ship;
		private SolarSystem _solarSystem;
		private AsteroidBelt _asteroid;
		private AsteroidBelt _difficultAsteroid;

		[SetUp]
		public void SetUp()
		{
			_solarSystem = new SolarSystem();
			_asteroid = new AsteroidBelt { Ore = ItemCode.Veldnium, Difficulty = 1, Richness = 500, AmountRemaining = 1000, Position = new Position(_solarSystem, Vector.Parse("1,0")) };
			_difficultAsteroid = new AsteroidBelt { Ore = ItemCode.Veldnium, Difficulty = 2, Richness = 500, AmountRemaining = 1000, Position = new Position(_solarSystem, Vector.Parse("1,0")) };
			_solarSystem.Satellites.Add(_asteroid);

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
			var missileLauncher = TestItems.Weapon(ItemCode.MissileLauncher);
			var miningLaser = TestItems.Weapon(ItemCode.MiningLaser);

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
			mining.SetParameters(new MineAsteroidParameters(_ship, _asteroid));
			var result = (MiningResult )mining.Execute();

			Assert.That(result.Success, Is.True);
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.Success));
			Assert.That(result.Ore.Quantity, Is.EqualTo(2));
			Assert.That(_asteroid.AmountRemaining, Is.EqualTo(998));
		}

		[Test]
		public void CanMakeAsteroidSmaller()
		{
			_asteroid.Reduce(100);
			Assert.That(_asteroid.AmountRemaining, Is.EqualTo(900));

			var amount = _asteroid.Reduce(1000);
			Assert.That(amount, Is.EqualTo(900));
			Assert.That(_asteroid.AmountRemaining, Is.EqualTo(0));
		}

		[Test]
		public void AsteroidHasDifficulty()
		{
			var veldnium = new ItemInfo(ItemCode.Veldnium);
			var itemFactory = new Moq.Mock<IItemFactory>(MockBehavior.Strict);
			itemFactory.Setup(x => x.Build(ItemCode.Veldnium, It.IsAny<uint>()))
				.Returns<ItemCode, uint>((item, quantity) => new Item(veldnium, quantity));

			var mining = new MiningTask(itemFactory.Object) { };
			mining.SetParameters(new MineAsteroidParameters(_ship, _difficultAsteroid));
			
			var result = mining.Mine();
			Assert.That(result.Success, Is.True);
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.Pending));
			Assert.That(_difficultAsteroid.AmountRemaining, Is.EqualTo(1000));
			Assert.That(_ship.Tasks, Contains.Item(mining));

			result = mining.Mine();

			Assert.That(result.Success, Is.True);
			Assert.That(result.StatusCode, Is.EqualTo(ShipTask.StatusCode.Success));

			Assert.That(result.Ore.Quantity, Is.EqualTo(2));
			Assert.That(_difficultAsteroid.AmountRemaining, Is.EqualTo(998));
			_ship.Tasks.ShouldNotContain(mining);
		}
	}
}