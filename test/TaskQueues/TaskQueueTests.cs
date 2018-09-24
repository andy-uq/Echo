using Echo.Tasks;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.TaskQueues
{
	[TestFixture]
	public class TaskQueueTests
	{
		[Test]
		public void AddTaskQueue()
		{
			var mock = new Mock<ITask>();

			var queue = new TaskQueue();
			queue.Add(mock.Object);

			Assert.That(queue.Count, Is.EqualTo(1));
		}

		[Test]
		public void TickTaskWithNoRemainingTime()
		{
			var result = new Mock<ITaskResult>(MockBehavior.Strict);
			var mock = new Mock<ITask>(MockBehavior.Strict);

			mock.Setup(x => x.Execute()).Returns(result.Object);
			result.SetupGet(f => f.TimeRemaining).Returns(0);

			var queue = new TaskQueue();
			queue.Add(mock.Object);

			var count = queue.Tick();
			Assert.That(count, Is.EqualTo(1));
			Assert.That(queue.Count, Is.EqualTo(0));
		}

		[Test]
		public void TickTaskWithRemainingTime()
		{
			var result = new Mock<ITaskResult>(MockBehavior.Strict);
			var mock = new Mock<ITask>(MockBehavior.Strict);

			mock.Setup(x => x.Execute()).Returns(result.Object);
			result.SetupGet(f => f.TimeRemaining).Returns(1);

			var queue = new TaskQueue();
			queue.Add(mock.Object);
			
			var count = queue.Tick();
			Assert.That(count, Is.EqualTo(0));
			Assert.That(queue.Count, Is.EqualTo(1));
		}
	}
}