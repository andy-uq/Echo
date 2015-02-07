using System;
using EnsureThat;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class EnsureThatTests
	{
		private class A
		{
			public string Result { get; set; }
		}

		[Test, ExpectedException(typeof(ArgumentException), UserMessage = "param must not equal 0")]
		public void IsNotEqualTo()
		{
			NotZero(1);
			NotZero(0);
		}

		[Test, ExpectedException(typeof(ArgumentException), UserMessage = "result must be null")]
		public void IsNull()
		{
			Null(new A());
			Null(new A() { Result = "OK" });
		}

		private void Null(A param)
		{
			Ensure.That(param.Result).IsNull();
		}

		private void NotZero(int param)
		{
			Ensure.That(param).IsNotEqualTo(0);
		}
	}
}