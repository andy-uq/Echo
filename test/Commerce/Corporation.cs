using Echo.Corporations;
using NUnit.Framework;

namespace Echo.Tests.Commerce
{
	[TestFixture]
	public class CorporationTests
	{
		[Test]
		public void ObjectTypeIsCorporation()
		{
			var corp = new Corporation();
			Assert.That(corp.ObjectType, Is.EqualTo(ObjectType.Corporation));
		}
	}
}