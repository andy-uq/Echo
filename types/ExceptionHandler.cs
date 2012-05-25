using System;

namespace Echo
{
	public static class ExceptionHandler
	{
		private static readonly NullExceptionHandler _nullExceptionHandler = new NullExceptionHandler();
		private static IExceptionHandler _exceptionHandler = null;

		public static void Initialise(IExceptionHandler exceptionHandler)
		{
			_exceptionHandler = exceptionHandler;
		}

		public static Exception Warn(Exception exception)
		{
			return (_exceptionHandler ?? _nullExceptionHandler).Warn(exception);
		}

		#region Nested type: NullExceptionHandler

		private class NullExceptionHandler : IExceptionHandler
		{
			#region IExceptionHandler Members

			Exception IExceptionHandler.Warn(Exception exception)
			{
				throw exception;
			}

			#endregion
		}

		#endregion
	}

	public interface IExceptionHandler
	{
		Exception Warn(Exception exception);
	}
}