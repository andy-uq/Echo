using System.Collections.Generic;
using Autofac;
using Echo.Tests.Infrastructure;
using NUnit.Framework;
using SisoDb;

namespace Echo.Data.Tests
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
			var configuration = new Configure();
			var configurationBuilder = new Autofac.ContainerBuilder();
			
			configuration.Build(configurationBuilder);
			var resolver = configurationBuilder.Build();

			var dbA = resolver.Resolve<ISisoDatabase>();
			var dbB = resolver.Resolve<ISisoDatabase>();

			Assert.That(dbA, Is.SameAs(dbB));
		}

		[Test]
		public void CanCreateDb()
		{
			using ( var fresh = new CreateFreshDatabase("empty") )
			{
				var configurationBuilder = new Autofac.ContainerBuilder();

				fresh.Create(configurationBuilder);
				var resolver = configurationBuilder.Build();

				var dbA = resolver.Resolve<ISisoDatabase>();
				var dbB = resolver.Resolve<ISisoDatabase>();

				Assert.That(dbA, Is.SameAs(dbB));
			}
		}

		[Test, Ignore("Siso does not support nested structures (Causes infinite loop)")]
		public void CanCreateNestedStructure()
		{
			using ( var fresh = new CreateFreshDatabase("db_for_a") )
			{
				var configurationBuilder = new Autofac.ContainerBuilder();

				fresh.Create(configurationBuilder);
				var resolver = configurationBuilder.Build();

				var dbA = resolver.Resolve<ISisoDatabase>();
				dbA.UseOnceTo().Insert(new A() {Name = "Bob"});
			}
		}
	}
}