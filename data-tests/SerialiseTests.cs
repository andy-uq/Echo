using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Client.Documents;

namespace Echo.Data.Tests
{
	[TestFixture]
	public class SerialiseTests
	{
		private IDisposable _databaseHandle;
		private JsonSerializer _serialiser;

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
			var fresh = new CreateFreshDatabase();
			var configurationBuilder = new Autofac.ContainerBuilder();

			fresh.Create(configurationBuilder);
			var resolver = configurationBuilder.Build();

			_serialiser = resolver.Resolve<IDocumentStore>().Conventions.CreateSerializer();
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
			var json = new StringWriter();
			_serialiser.Serialize(json, item);
			Console.WriteLine(json);
		}

		[Test]
		public void CanSerialiseNullableStruct()
		{
			var item = new C { Value = new D { X = "Bob" } };
			var json = new StringWriter();
			_serialiser.Serialize(json, item);
			Console.WriteLine(json);
		}

		[Test]
		public void CanDeserialiseNullableStruct()
		{
			var reader = new JsonTextReader(new StringReader("{\"Value\":{\"X\":\"Bob\"}}"));
			var i2 = _serialiser.Deserialize<C>(reader);

			Assert.That(i2, Is.Not.Null);
			Assert.That(i2.Value, Is.Not.Null);
			Assert.That(((D)i2.Value).X, Is.EqualTo("Bob"));
		}

		[Test]
		public void CanSerialiseInheritedObject()
		{
			var item = new B() { Id = 10, Name = "B", Children = new List<A>() { new A() { Name = "Child A", }, new A{ Name = "Child B" }} };
			var json = new StringWriter();
			_serialiser.Serialize(json, item);
			Console.WriteLine(json);
		}

		[Test, Ignore("Type information required")]
		public void CanDeserialiseInheritedObject()
		{
			var item = new A() { Name = "B", Children = new List<A>() { new B() { Id = 10, Name = "Child B", }, new A { Name = "Child A" } } };
			var json = new StringWriter();
			_serialiser.Serialize(json, item);

			var reader = new JsonTextReader(new StringReader(json.ToString()));
			var b = _serialiser.Deserialize<A>(reader);

			Assert.That(b.Children.First(), Is.InstanceOf<B>());
			Assert.That(((B)b.Children.First()).Id, Is.EqualTo(10));
		}
	}
}