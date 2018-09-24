using Echo.Items;
using Echo.Items.Packing;
using Echo.Ships;
using Echo.State;
using Echo.Tests.Mocks;
using NUnit.Framework;
using test.common;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class ItemFactoryTests
	{
		[Test]
		public void BuildOre()
		{
			var itemFactory = new ItemFactory(new IdResolutionContext(new[] { new ItemInfo(ItemCode.Veldnium) }));
			var ore = itemFactory.Build(ItemCode.Veldnium, 10);

			Assert.That(ore.ItemInfo.Code, Is.EqualTo(ItemCode.Veldnium));
			Assert.That(ore.Quantity, Is.EqualTo(10));
		}

		[Test]
		public void BuildWeapon()
		{
			var itemFactory = new ItemFactory(new IdResolutionContext(new[] { TestItems.Item(ItemCode.MissileLauncher), TestItems.Weapon(ItemCode.MissileLauncher) }));
			var weaponPacker = new WeaponPacker(new IdGenerator(), itemFactory);
			var itemPacker = new ItemPacker(new[] { weaponPacker });

			var weaponInBox = itemFactory.Build(ItemCode.MissileLauncher, 10);
			var weapon = itemPacker.Unpack<Weapon>(weaponInBox);

			Assert.That(weapon, Is.Not.Null);
			Assert.That(weapon.WeaponInfo.Code, Is.EqualTo(ItemCode.MissileLauncher));
			Assert.That(weapon.Id, Is.Not.EqualTo(0));
			Assert.That(weaponInBox.Quantity, Is.EqualTo(9));
		}

		[Test]
		public void PackWeaponInBox()
		{
			var itemFactory = new ItemFactory(new IdResolutionContext(new[] { TestItems.Item(ItemCode.MissileLauncher) }));
			var weaponPacker = new WeaponPacker(new IdGenerator(), itemFactory);
			var itemPacker = new ItemPacker(new[] { weaponPacker });
			var weapon = new Weapon { WeaponInfo = new WeaponInfo { Code = ItemCode.MissileLauncher} };

			Assert.That(itemPacker.CanPack(weapon), Is.True);
			var weaponInBox = itemPacker.Pack(weapon);

			Assert.That(weaponInBox, Is.Not.Null);
			Assert.That(weaponInBox.ItemInfo, Is.InstanceOf<WeaponInfo>());
		}
	}
}