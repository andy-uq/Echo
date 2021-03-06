﻿using System.Collections.Generic;
using Autofac;
using Echo.Data;
using NUnit.Framework;
using Raven.Client.Documents;

namespace Echo.Tests
{
	[TestFixture]
	public class ConfigureDataTests
	{
		public class A
		{
			public string Name { get; set; }
			public List<A> Children { get; set; }
		}

		[Test]
		public void CanConfigureFromAppConfig()
		{
			var configurationBuilder = new ContainerBuilder();
			
			var configuration = new Configure();
			configuration.Build(configurationBuilder);

			using ( var resolver = configurationBuilder.Build() )
			{
				var dbA = resolver.Resolve<IDocumentStore>();
				var dbB = resolver.Resolve<IDocumentStore>();

				Assert.That(dbA, Is.SameAs(dbB));
			}
		}

		[Test]
		public void CanCreateDb()
		{
			using ( var fresh = new CreateFreshDatabase() )
			{
				var configurationBuilder = new ContainerBuilder();

				fresh.Create(configurationBuilder);
				using ( var resolver = configurationBuilder.Build() )
				{
					var dbA = resolver.Resolve<IDocumentStore>();
					var dbB = resolver.Resolve<IDocumentStore>();

					Assert.That(dbA, Is.SameAs(dbB));
				}
			}
		}

		[Test]
		public void CanCreateNestedStructure()
		{
			using ( var fresh = new CreateFreshDatabase() )
			{
				var configurationBuilder = new ContainerBuilder();
				fresh.Create(configurationBuilder);

				using ( var resolver = configurationBuilder.Build() )
				{
					var dbA = resolver.Resolve<IDocumentStore>();
					using (var session = dbA.OpenSession())
					{
						session.Store(new A {Name = "Bob"});
					}
				}
			}
		}
	}
}