using System;

namespace Echo
{
	public class ExceptionHandler
	{
		private static IExceptionHandler _exceptionHandler = new NullExceptionHandler();

		private class NullExceptionHandler : IExceptionHandler
		{
			public Exception Warn(Exception exception)
			{
				throw exception;
			}
		}

		static ExceptionHandler() { }

		private ExceptionHandler()
		{}

		public static void Initialise(IExceptionHandler exceptionHandler)
		{
			_exceptionHandler = exceptionHandler;
		}

		public static Exception Warn(Exception exception)
		{
			return _exceptionHandler.Warn(exception);
		}
	}

	public interface IExceptionHandler
	{
		Exception Warn(Exception exception);
	}
}