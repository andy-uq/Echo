using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class ItemTests
	{
		[Test]
		public void ParseBoxedItemCode()
		{
			ItemCode itemCode;

			const long badId = 1L;
			Assert.That(badId.TryParse(out itemCode), Is.False);

			const long goodId = (long)ItemCode.MissileLauncher ^ ItemCodeExtensions.GENERIC_ITEM_ID_MASK;
			Assert.That(goodId.TryParse(out itemCode), Is.True);
			Assert.That(itemCode, Is.EqualTo(ItemCode.MissileLauncher));
		}

		[Test]
		public void ParseWeaponItemCode()
		{
			ItemCode itemCode;
			
			const long goodId = (long)ItemCode.MissileLauncher ^ ItemCodeExtensions.WEAPON_ID_MASK;
			Assert.That(goodId.TryParse(out itemCode), Is.True);
			Assert.That(itemCode, Is.EqualTo(ItemCode.MissileLauncher));
		}

		[Test]
		public void ParseBluePrintItemCode()
		{
			ItemCode itemCode;
			
			const long goodId = (long)ItemCode.MissileLauncher ^ ItemCodeExtensions.BLUEPRINT_ID_MASK;
			Assert.That(goodId.TryParse(out itemCode), Is.True);
			Assert.That(itemCode, Is.EqualTo(ItemCode.MissileLauncher));
		}
		
		[Test]
		public void GetItemCategories()
		{
			var categories = ItemCode.LightFrigate.GetItemCategories();
			Assert.That(categories, Is.Not.Empty);
			Assert.That(categories, Is.EquivalentTo(new[] { ItemType.Ships }));
		}

		[Test]
		public void ItemNameIsItemInfoName()
		{
			var itemInfo = new ItemInfo {Name = "Name of item"};
			var item = new Item(itemInfo);

			Assert.That(item.Name, Is.EqualTo(itemInfo.Name));
		}
	}
}