using System;

namespace Echo.State
{
	public class JumpGateState : IObjectState
	{
		public Guid Id { get; set; }
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public long ConnectsTo { get; set; }
	}
}