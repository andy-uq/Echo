namespace Echo.Tasks
{
	public interface ITask
	{
		ITaskResult Execute(ITaskParameters taskParameters);
	}
}