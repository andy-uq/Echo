using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using SisoDb;
using SisoDb.Serialization;

namespace Echo.Data.Tests
{
	[TestFixture]
	public class SerialiseTests
	{
		private ISisoSerializer _serialiser;
		private IDisposable _databaseHandle;

		public class A
		{
			public string Name { get; set; }
			public List<A> Children { get; set; }
		}

		public class B : A
		{
			public int Id { get; set; }
		}

		public class C
		{
			public D? Value { get; set; }
		}

		public struct D
		{
			public static D Parse(string value)
			{
				return new D { X = value };
			}

			public string X { get; set; }

			public override string ToString()
			{
				return X;
			}
		}

		[SetUp]
		public void SetUp()
		{
			var fresh = new CreateFreshDatabase("serialise-tests");
			var configurationBuilder = new Autofac.ContainerBuilder();

			fresh.Create(configurationBuilder);
			var resolver = configurationBuilder.Build();

			_serialiser = resolver.Resolve<ISisoDatabase>().Serializer;
			_databaseHandle = fresh;
		}

		[TearDown]
		public void TearDown()
		{
			if (_databaseHandle != null)
				_databaseHandle.Dispose();
		}

		[Test]
		public void CanSerialiseNestedObject()
		{
			var item = new A() {Name = "A", Children = new List<A>() { new A() { Name = "Child A", }, new A{ Name = "Child B" }} };
			var json = _serialiser.Serialize(item);
			Console.WriteLine(json);
		}

		[Test]
		public void CanSerialiseNullableStruct()
		{
			var item = new C { Value = new D { X = "Bob" } };
			var json = _serialiser.Serialize(item);
			Console.WriteLine(json);
		}

		[Test, Ignore("Cannot deserialise nullable struct")]
		public void CanDeserialiseNullableStruct()
		{
			var item = new C { Value = new D() };
			var json = _serialiser.Deserialize<C>("{\"Value\":\"Bob\"}");
			Assert.That(json.Value, Is.Not.Null);
		}

		[Test]
		public void CanSerialiseInheritedObject()
		{
			var item = new B() { Id = 10, Name = "B", Children = new List<A>() { new A() { Name = "Child A", }, new A{ Name = "Child B" }} };
			var json = _serialiser.Serialize(item);
			Console.WriteLine(json);
		}

		[Test, Ignore("Cannot deserialise polymorhphic objects")]
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