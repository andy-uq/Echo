namespace Echo.Tasks
{
	public interface ITask
	{
		ITaskResult SetParameters(ITaskParameters taskParameters);
		ITaskResult Execute();
	}
}