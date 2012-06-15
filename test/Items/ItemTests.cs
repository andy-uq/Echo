using Echo.Items;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class ItemTests
	{
		[Test]
		public void GetItemCategories()
		{
			var categories = ItemCode.LightFrigate.GetItemCategories();
			Assert.That(categories, Is.Not.Empty);
			Assert.That(categories, Is.EquivalentTo(new[] { ItemCategory.Ships }));
		}
	 
	}
}