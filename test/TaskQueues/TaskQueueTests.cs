using Echo.Tasks;
using NUnit.Framework;

namespace Echo.Tests.TaskQueues
{
	[TestFixture]
	public class TaskQueueTests
	{
		[Test]
		public void AddTaskQueue()
		{
			var mock = new Moq.Mock<ITask>();

			var queue = new TaskQueue();
			queue.Add(mock.Object);

			Assert.That(queue.Count, Is.EqualTo(1));
		}

		[Test]
		public void Tick()
		{
			var mock = new Moq.Mock<ITask>();

			var queue = new TaskQueue();
			queue.Add(mock.Object);

			int count = queue.Tick();
			Assert.That(count, Is.EqualTo(1));
			Assert.That(queue.Count, Is.EqualTo(0));

			queue.Add(mock.Object);
			Assert.That(count, Is.EqualTo(0));
			Assert.That(queue.Count, Is.EqualTo(1));
		}
	}
}