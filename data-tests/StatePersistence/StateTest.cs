﻿using Autofac;
using Echo.Tests.Mocks;
using NUnit.Framework;
using Raven.Client.Documents;

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
			var configurationBuilder = new ContainerBuilder();

			var fresh = new CreateFreshDatabase();
			fresh.Create(configurationBuilder);

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