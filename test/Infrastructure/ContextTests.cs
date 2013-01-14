using Echo.Mapping;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ContextTests
	{
		private ITypeMapper _typeMapper;
		private IRandom _random;

		public EchoContext Create()
		{
			return new EchoContext(_typeMapper, _random);
		}

		[Test]
		public void CreateContext()
		{
			_typeMapper = new Moq.Mock<ITypeMapper>().Object;
			_random = new Moq.Mock<IRandom>().Object;

			var context = Create();

			Assert.That(context.TypeMapper, Is.Not.Null);
			Assert.That(context.Random, Is.Not.Null);
		}
	}
}