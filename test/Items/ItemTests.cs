using Echo.Items;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class ItemTests
	{
		[Test]
		public void ParseItemCode()
		{
			ItemCode itemCode;

			const long badId = 1L;
			Assert.That(badId.TryParse(out itemCode), Is.False);

			const long goodId = (long)ItemCode.LightFrigate ^ ItemCodeExtensions.ITEM_ID_MASK;
			Assert.That(goodId.TryParse(out itemCode), Is.True);
			Assert.That(itemCode, Is.EqualTo(ItemCode.LightFrigate));
		}
		
		[Test]
		public void GetItemCategories()
		{
			var categories = ItemCode.LightFrigate.GetItemCategories();
			Assert.That(categories, Is.Not.Empty);
			Assert.That(categories, Is.EquivalentTo(new[] { ItemCategory.Ships }));
		}
	}
}