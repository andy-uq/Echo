using common.Interfaces;
using core;
using core.State;
using Ninject;
using NUnit.Framework;

namespace nunit
{
	[TestFixture]
	public class Start
	{

		[Test]
		public void ObjectsGetIDs()
		{
			ServiceLocator.Bind<IIDGenerator>().To<IDGenerator>();

			var e = new Entity();
			Assert.AreEqual(1, e.ID, "The first object created did not have the correct ID");
		}

		[Test]
		public void ObjectsHaveUniqueIDs()
		{
			ServiceLocator.Bind<IIDGenerator>().To<IDGenerator>().InSingletonScope();
			ServiceLocator.Bind<IEntity>().To<Entity>();

			var e1 = ServiceLocator.Get<IEntity>();
			Assert.AreEqual(1, e1.ID);

			var e2 = ServiceLocator.Get<IEntity>();
			Assert.AreEqual(2, e2.ID, "Second object did not get a unique id");

			int expected = 3;
			for (int i=0;i<1000;i++)
			{
				var eN = ServiceLocator.Get<IEntity>();
				Assert.AreEqual(expected, eN.ID, "Non-sequential IDs");
				expected++;
			}
		}
	}
}