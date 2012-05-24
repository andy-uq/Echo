using System;

namespace Echo
{
	public abstract class DisposableObject : IDisposable
	{
		protected bool IsDisposed { get; private set; }

		~DisposableObject()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);

		}

		private void Dispose(bool disposing)
		{
			if (IsDisposed) 
				return;

			if ( disposing )
			{
				try
				{
					DisposeManagedResources();
				}
				catch (Exception ex)
				{
					ExceptionHandler.Warn(ex);
				}
			}

			DisposeUnmanagedResources();
			IsDisposed = true;
		}

		protected abstract void DisposeManagedResources();

		protected virtual void DisposeUnmanagedResources()
		{}
	}
}