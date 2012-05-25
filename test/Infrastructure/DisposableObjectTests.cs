using System;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class DisposableObjectTests
	{
		 class TestDisposableObject : DisposableObject
		 {
		 	protected override void DisposeUnmanagedResources()
		 	{
				DisposeUnmanagedResourcesCount++;
				base.DisposeUnmanagedResources();
			}

		 	protected override void DisposeManagedResources()
		 	{
		 		DisposeManagedResourcesCount++;
		 	}

			public int DisposeUnmanagedResourcesCount { get; set; }
		 	public int DisposeManagedResourcesCount { get; set; }
		 }

		 class ThrowingDisposableObject : TestDisposableObject
		{
			protected override void DisposeManagedResources()
			{
				base.DisposeManagedResources();
				throw new Exception("Bad object!");
			}
		}

		[Test]
		public void DeterministicDisposal()
		{
			var t = new TestDisposableObject();
			t.Dispose();
			Assert.That(t.DisposeUnmanagedResourcesCount, Is.EqualTo(1));			
			Assert.That(t.DisposeManagedResourcesCount, Is.EqualTo(1));			
		}

		[Test]
		public void MultipleDisposal()
		{
			var t = new TestDisposableObject();
			t.Dispose();
			t.Dispose();
			Assert.That(t.DisposeUnmanagedResourcesCount, Is.EqualTo(1));			
			Assert.That(t.DisposeManagedResourcesCount, Is.EqualTo(1));			
		}

		[Test]
		public void SafeDisposal()
		{
			var exceptionHandler = new Moq.Mock<IExceptionHandler>(MockBehavior.Strict);
			exceptionHandler.Setup(x => x.Warn(It.IsAny<Exception>())).Returns<Exception>(x => x).Verifiable();
			
			ExceptionHandler.Initialise(exceptionHandler.Object);

			var t = new ThrowingDisposableObject();
			t.Dispose();
			
			Assert.That(t.DisposeUnmanagedResourcesCount, Is.EqualTo(1));			
			Assert.That(t.DisposeManagedResourcesCount, Is.EqualTo(1));			
			exceptionHandler.Verify(x => x.Warn(It.IsAny<Exception>()), Times.Once());
		}
	}
}