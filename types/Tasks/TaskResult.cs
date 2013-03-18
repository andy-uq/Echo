namespace Echo.Tasks
{
	public abstract class TaskResult
	{
		public string Task { get; set; }
		public bool Success { get; set; }
		public uint TimeRemaining { get; set; }
	}
}