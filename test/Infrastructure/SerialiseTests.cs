using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using SisoDb;
using SisoDb.Serialization;

namespace Echo.Tests.Infrastructure
{
	public class SerialiseTests
	{
		private IJsonSerializer _serialiser;

		public class A
		{
			public string Name { get; set; }
			public List<A> Children { get; set; }
		}

		public class B : A
		{
			public int Id { get; set; }
		}

		[SetUp]
		public void SetUp()
		{
			var fresh = new CreateFreshDatabase("empty");
			var configurationBuilder = new Autofac.ContainerBuilder();

			fresh.Create(configurationBuilder);
			var resolver = configurationBuilder.Build();

			_serialiser = resolver.Resolve<ISisoDatabase>().Serializer;
		}

		[Test]
		public void CanSerialiseNestedObject()
		{
			var item = new A() {Name = "A", Children = new List<A>() { new A() { Name = "Child A", }, new A{ Name = "Child B" }} };
			var json = _serialiser.Serialize(item);
			Console.WriteLine(json);
		}

		[Test]
		public void CanSerialiseInheritedObject()
		{
			var item = new B() { Id = 10, Name = "B", Children = new List<A>() { new A() { Name = "Child A", }, new A{ Name = "Child B" }} };
			var json = _serialiser.Serialize(item);
			Console.WriteLine(json);
		}

		[Test, Ignore("Service stack cannot deserialise polymorhphic objects")]
		public void CanDeserialiseInheritedObject()
		{
			var item = new A() { Name = "B", Children = new List<A>() { new B() { Id = 10, Name = "Child B", }, new A { Name = "Child A" } } };
			var json = _serialiser.Serialize(item);

			var b = _serialiser.Deserialize<A>(json);
			Assert.That(b.Children.First(), Is.InstanceOf<B>());
			Assert.That(((B)b.Children.First()).Id, Is.EqualTo(10));
		}
	}
}