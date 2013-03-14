using System.Linq;
using Echo.Exceptions;
using Echo.Items;
using Echo.State;
using Echo.Tests.Mocks;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class IdResolutionTests
	{
		[Test,ExpectedException(typeof(ItemNotFoundException))]
		public void IdResolutionFail()
		{
			var itemInfo = TestItems.Item(ItemCode.MissileLauncher);

			var idResolution = new IdResolutionContext(new[] { itemInfo, });
			var objRef = new ObjectReference(1L);
			idResolution.Get<ItemInfo>(objRef);
		}

		[Test]
		public void IdResolutionGet()
		{
			var itemInfo = TestItems.Item(ItemCode.MissileLauncher);

			var idResolution = new IdResolutionContext(new[] { itemInfo, });
			var objRef = new ObjectReference(itemInfo.ObjectId);
			idResolution.Get<ItemInfo>(objRef);
		}

		[Test]
		public void IdResolutionCombine()
		{
			var i1 = TestItems.Item(ItemCode.MissileLauncher);
			var i2 = TestItems.Item(ItemCode.EnergyShield);

			var idResolution = new IdResolutionContext(new[] { i1, }).Combine(new IdResolutionContext(new[] { i2, }));
			
			var objRef = new ObjectReference(i2.ObjectId);
			idResolution.Get<ItemInfo>(objRef);
		}
	}
}