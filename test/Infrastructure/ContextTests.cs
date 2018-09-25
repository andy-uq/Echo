using Echo.Mapping;
using Echo.Ships;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using test.common;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ContextTests
	{
		private ITypeMapper _typeMapper;
		private IRandom _random;
		private IIdGenerator _idGenerator;
		private IIdResolver _resolver;

		private EchoContext Create()
		{
			return new EchoContext(_typeMapper, _random, _idGenerator, _resolver);
		}

		[Test]
		public void CreateContext()
		{
			_typeMapper = new Mock<ITypeMapper>().Object;
			_random = new Mock<IRandom>().Object;
			_idGenerator = new Mock<IIdGenerator>().Object;
			_resolver = new TestIdResolver();

			var context = Create();

			Assert.That(context.TypeMapper, Is.Not.Null);
			Assert.That(context.Random, Is.Not.Null);
			Assert.That(context.IdGenerator, Is.Not.Null);
			Assert.That(context.Resolver, Is.Not.Null);
		}
	}
}