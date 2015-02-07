using Echo.Agents;
using Echo.Items;
using Echo.State;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ObjectTests
	{
		[Test]
		public void Item()
		{
			var item = new Item(new ItemInfo());
			item.ObjectType.ShouldBe(ObjectType.Item);
		}

		[Test]
		public void Agent()
		{
			var item = new Agent();
			item.ObjectType.ShouldBe(ObjectType.Agent);
		}
	}
}