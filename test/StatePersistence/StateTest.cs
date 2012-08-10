using System;
using System.Linq;
using Echo.Data;
using Echo.Tests.Infrastructure;
using NUnit.Framework;
using Raven.Client;
using test;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class StateTest
	{
		protected IDocumentStore Database { get; set; }
		protected MockUniverse Universe { get; set; }

		[SetUp]
		public virtual void SetUp()
		{
			var fresh = new Configure();
			var configurationBuilder = new Autofac.ContainerBuilder();

			fresh.Build(configurationBuilder);
			//fresh.Create(configurationBuilder);

			var resolver = new AutofacResolver(configurationBuilder);

			Database = resolver.Resolve<IDocumentStore>();
			Universe = new MockUniverse();
		}

		[TearDown]
		public void CleanUp()
		{
			Database.Dispose();
			Database = null;
		}

		protected void DumpObjects(string name, bool isInfo = false)
		{
			
		}
	}
}