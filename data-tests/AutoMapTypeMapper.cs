﻿using AutoMapper;
using Echo.Mapping;
using NUnit.Framework;

namespace Echo.Tests
{
	[TestFixture]
	public class AutoMapTypeMapperTests
	{
		private class A
		{
			public string Name { get; set; }
		}

		private class B
		{
			public string Name { get; set; }
		}

		[Test]
		public void CanMap()
		{
			Mapper.Initialize(config => { config.CreateMap<A, B>(); });

			var value = new A { Name = "A" };

			ITypeMapper mapper = new AutoMapTypeMapper(Mapper.Instance);
			var b = mapper.Map<B>(value);
			Assert.That(b.Name,Is.EqualTo("A"));


			var result = mapper.Map(value, typeof(B));
			Assert.That(result, Is.InstanceOf<B>());
			Assert.That(result, Has.Property("Name").EqualTo("A"));
		}
	}
}