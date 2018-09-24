using Echo.Mapping;
using Echo.Ships;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ContextTests
	{
		private ITypeMapper _typeMapper;
		private IRandom _random;
		private IIdGenerator _idGenerator;

		private EchoContext Create()
		{
			return new EchoContext(_typeMapper, _random, _idGenerator);
		}

		[Test]
		public void CreateContext()
		{
			_typeMapper = new Mock<ITypeMapper>().Object;
			_random = new Mock<IRandom>().Object;
			_idGenerator = new Mock<IIdGenerator>().Object;

			var context = Create();

			Assert.That(context.TypeMapper, Is.Not.Null);
			Assert.That(context.Random, Is.Not.Null);
			Assert.That(context.IdGenerator, Is.Not.Null);
		}
	}
}