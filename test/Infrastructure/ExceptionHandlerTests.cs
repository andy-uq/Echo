using System;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ExceptionHandlerTests
	{
		[Test, ExpectedException(typeof(Exception), ExpectedMessage = "Rethrow")]
		public void DefaultExceptionHandlerThrows()
		{
			ExceptionHandler.Initialise(null);
			ExceptionHandler.Warn(new Exception("Rethrow"));
		}
	}
}