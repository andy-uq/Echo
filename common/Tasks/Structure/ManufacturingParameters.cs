using Echo.Agents;
using Echo.State;

namespace Echo.Tasks.Structure
{
	public class ManufacturingParameters : ITaskParameters
	{
		public BluePrintInfo BluePrint { get; set; }
		public Agent Agent { get; set; }
	}
}