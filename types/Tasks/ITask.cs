namespace Echo.Tasks
{
	public interface ITask : IObject
	{
		ITaskResult SetParameters(ITaskParameters taskParameters);
		ITaskResult Execute();
	}
}