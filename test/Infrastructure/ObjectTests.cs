using Echo.Agents;
using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ObjectTests
	{
		[Test]
		public void Item()
		{
			var item = new Item(new ItemInfo());
			Assert.That(item.ObjectType, Is.EqualTo(ObjectType.Item));
		}

		[Test]
		public void Agent()
		{
			var item = new Agent();
			Assert.That(item.ObjectType, Is.EqualTo(ObjectType.Agent));
		}

		
	}
}