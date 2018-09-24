namespace Echo.State
{
	public class JumpGateState : IObjectState
	{
		public ulong ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public ulong ConnectsTo { get; set; }
	}
}