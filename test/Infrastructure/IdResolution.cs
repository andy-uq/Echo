using System.Linq;
using Echo.Exceptions;
using Echo.Items;
using Echo.State;
using NUnit.Framework;
using Shouldly;
using test.common;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class IdResolutionTests
	{
		[Test]
		public void IdResolutionFail()
		{
			const long id = 1L;
			var itemInfo = TestItems.Item(ItemCode.MissileLauncher);

			var idResolution = new IdResolutionContext(new[] { itemInfo });
			var objRef = new ObjectReference(id);

			Should.Throw<ItemNotFoundException>(() => idResolution.Get<ItemInfo>(objRef)).Message.ShouldBe("Could not find ItemInfo \"[x00000001]\"");
			Should.Throw<ItemNotFoundException>(() => idResolution.GetById<ItemInfo>(id)).Message.ShouldBe("Could not find ItemInfo \"1\"");
		}

		[Test]
		public void SupportsNulls()
		{
			var idResolution = new IdResolutionContext(Enumerable.Empty<IObject>());

			idResolution.Get<ItemInfo>(null).ShouldBe(null);
			idResolution.TryGet(null, out ItemInfo value).ShouldBe(false);
		}

		[Test]
		public void IdResolutionGet()
		{
			var itemInfo = TestItems.Item(ItemCode.MissileLauncher);

			var idResolution = new IdResolutionContext(new[] { itemInfo });
			var objRef = new ObjectReference(itemInfo.ObjectId);
			idResolution.Get<ItemInfo>(objRef);
		}

		[Test]
		public void IdResolutionCombine()
		{
			var i1 = TestItems.Item(ItemCode.MissileLauncher);
			var i2 = TestItems.Item(ItemCode.EnergyShield);

			var r1 = new IdResolutionContext(new[] { i1 });
			var r2 = new IdResolutionContext(new[] { i2 });
			
			var idResolution = r1.Combine(r2);
			idResolution.ShouldBeOfType<CompositeIdResolver>();
			
			var objRef = new ObjectReference(i2.ObjectId);
			idResolution.Get<ItemInfo>(objRef);

			idResolution.Values.ShouldContain(i1);
			idResolution.Values.ShouldContain(i2);
		}
	}
}