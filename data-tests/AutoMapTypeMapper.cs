using NUnit.Framework;
using AutoMapper;

namespace Echo.Data.Tests
{
	[TestFixture]
	public class AutoMapTypeMapperTests
	{
		private class A {}
		private class B {}

		[Test]
		public void CanMap()
		{
			Mapper.CreateMap<A, B>();

			var mapper = new AutoMapTypeMapper(Mapper.Engine);
			mapper.Map<B>(new A());
		}
		 
	}
}