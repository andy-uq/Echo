using Echo.Corporations;
using Echo.Items;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;

namespace Echo.Tests.Corporations
{
	[TestFixture]
	public class CorporationTests
	{
		[Test]
		public void CreateCorporation()
		{
			var corp = new Corporation();
			Assert.That(corp.ObjectType, Is.EqualTo(ObjectType.Corporation));
		}

		[Test]
		public void CorporationItems()
		{
			var corp = new Corporation();
			corp.Property.Add(new Item(new ItemInfo(ItemCode.LightFrigate)));
		}

		[Test]
		public void CorporationStructures()
		{
			var corp = new Corporation();
			corp.Structures.Add(new Manufactory());
		}
	}
}