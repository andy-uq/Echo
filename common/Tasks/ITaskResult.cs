namespace Echo
{
	public interface ITaskResult
	{
		string Task { get; }

		bool Success { get; }
		string ErrorCode { get; }
		object ErrorParams { get; }
	}
}