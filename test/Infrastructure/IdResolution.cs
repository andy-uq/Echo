using System.Linq;
using Echo.Exceptions;
using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class IdResolutionTests
	{
		[Test,ExpectedException(typeof(ItemNotFoundException))]
		public void IdResolutionFail()
		{
			var itemInfo = new ItemInfo {Code = ItemCode.MissileLauncher, Name = "Missile Launcher"};

			var idResolution = new IdResolutionContext(new[] { itemInfo, });
			var objRef = new ObjectReference(1L);
			idResolution.Get<ItemInfo>(objRef);
		}

		[Test]
		public void IdResolutionGet()
		{
			var itemInfo = new ItemInfo { Code = ItemCode.MissileLauncher, Name = "Missile Launcher" };

			var idResolution = new IdResolutionContext(new[] { itemInfo, });
			var objRef = new ObjectReference(itemInfo.ObjectId);
			idResolution.Get<ItemInfo>(objRef);
		}
	}
}