﻿using Autofac;
using Autofac.Core.Registration;
using Echo.Mapping;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests
{
	[TestFixture]
	public class DependencyInjectionInfrastructureTests
	{
		interface IFoo {}
		interface IFooBar : IFoo {}
		class A : IFoo {}
		class B : IFooBar {}

		class DemandA : IDemandBuilder
		{
			public void Build(ContainerBuilder containerBuilder)
			{
				containerBuilder.RegisterType<A>().As<IFoo>();
			}
		}

		class DemandB : IDemandBuilder
		{
			public void Build(ContainerBuilder containerBuilder)
			{
				containerBuilder.RegisterType<B>()
					.As<IFooBar>()
					.As<IFoo>();
			}
		}

		[Test]
		public void TestDependencyInjectionSucceeds()
		{
			var container = new ContainerBuilder();
			var builder = new DemandA();
			builder.Build(container);

			var resolver = new AutofacResolver(container.Build());

			Assert.That(resolver.Resolve<IFoo>(), Is.InstanceOf<A>());
		}

		[Test]
		public void TestDependencyInjectionOfDerivedTypeSucceeds()
		{
			var container = new ContainerBuilder();
			var builder = new DemandB();
			builder.Build(container);

			var resolver = new AutofacResolver(container.Build());

			Assert.That(resolver.Resolve<IFoo>(), Is.InstanceOf<B>());
			Assert.That(resolver.Resolve<IFooBar>(), Is.InstanceOf<B>());
		}

		[Test]
		public void TestDependencyInjectionCanBeMissing()
		{
			var container = new ContainerBuilder();
			var resolver = new AutofacResolver(container.Build());

			Assert.That(resolver.TryResolve(out IFoo value), Is.False);
		}

		[Test]
		public void TestDependencyInjectionThrowsWhenMissing()
		{
			var container = new ContainerBuilder();
			var resolver = new AutofacResolver(container.Build());

			Should.Throw<ComponentNotRegisteredException>(() => resolver.Resolve<IFoo>());
		}

	}
}