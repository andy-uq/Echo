using System;
using System.Collections.Generic;

namespace Echo.Tasks
{
	public class TaskQueue 
	{
		private List<ITask> _queue = new List<ITask>();

		public void Add(ITask task)
		{
			_queue.Add(task);
		}

		public int Count
		{
			get { return _queue.Count; }
		}

		public int Tick()
		{
			var requeue = new List<ITask>(_queue.Count);
			var count = 0;
			try
			{
				foreach ( var task in _queue )
				{
					var result = task.Execute();
					if (result.TimeRemaining == 0)
					{
						count++;
					}
					else
					{
						requeue.Add(task);
					}
				}
			}
			finally
			{
				_queue = requeue;
			}

			return count;
		}
	}
}