namespace Echo.Tasks
{
	public interface ITaskResult
	{
		uint TimeRemaining { get; }
		
		string Task { get; }

		bool Success { get; }
		string StatusCode { get; }
		object ErrorParams { get; }
	}
}