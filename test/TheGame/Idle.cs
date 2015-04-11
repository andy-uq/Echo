using System;
using Echo.Engine;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.TheGame
{
	public class Idle
	{
		[Test]
		public void Blank()
		{
			var idle = new IdleTimer();
			idle.Idle.ShouldBe(100);
		}

		[Test]
		public void OneSample()
		{
			var idle = new IdleTimer();
			idle.Enqueue(0.5);
			idle.Idle.ShouldBe(50);
		}

		[Test]
		public void TwoSample()
		{
			var idle = new IdleTimer();
			idle.Enqueue(0.25);
			idle.Enqueue(0.75);
			idle.Idle.ShouldBe(50);
			Console.WriteLine(idle.Idle);
		}

		[Test]
		public void FiveHundredSample()
		{
			var idle = new IdleTimer();

			for (var i = 0; i < 250; i++)
			{
				idle.Enqueue(0.25);
				idle.Enqueue(0.75);
			}
			
			idle.Idle.ShouldBe(50, 1);
			Console.WriteLine(idle.Idle);
		}
	}
}