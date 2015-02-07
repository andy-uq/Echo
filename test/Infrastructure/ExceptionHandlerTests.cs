using System;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ExceptionHandlerTests
	{
		[Test]
		public void DefaultExceptionHandlerThrows()
		{
			ExceptionHandler.Initialise(null);
			Should.Throw<Exception>(() => ExceptionHandler.Warn(new Exception("Rethrow")));
		}
	}
}