namespace Echo
{
	public interface ITaskResult
	{
		uint TicksRemaining { get; }
		
		string Task { get; }

		bool Success { get; }
		string ErrorCode { get; }
		object ErrorParams { get; }
	}
}