using System.Linq;
using Echo.Items;
using Echo.Items.Packing;
using Echo.Ships;
using Echo.State;
using Echo.Tests.Mocks;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class ItemFactoryTests
	{
		[Test]
		public void BuildOre()
		{
			var itemFactory = new ItemFactory(new IdResolutionContext(new[] { new ItemInfo(ItemCode.Veldnium) }), Enumerable.Empty<IItemPacker>());
			var ore = itemFactory.Build(ItemCode.Veldnium, 10);

			Assert.That(ore.ItemInfo.Code, Is.EqualTo(ItemCode.Veldnium));
			Assert.That(ore.Quantity, Is.EqualTo(10));
		}

		[Test]
		public void BuildWeapon()
		{
			var weaponPacker = new WeaponPacker(new IdGenerator());
			var itemFactory = new ItemFactory(new IdResolutionContext(new[] { new WeaponInfo { Code = ItemCode.MissileLauncher }, }), new[] { weaponPacker });
			var weaponInBox = itemFactory.Build(ItemCode.MissileLauncher, 10);

			var weapon = itemFactory.Unpack<Weapon>(weaponInBox);

			Assert.That(weapon, Is.Not.Null);
			Assert.That(weapon.WeaponInfo.Code, Is.EqualTo(ItemCode.MissileLauncher));
			Assert.That(weapon.Id, Is.Not.EqualTo(0));
		}
	}
}