namespace Echo.State
{
	public class JumpGateState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public long ConnectsTo { get; set; }
	}
}