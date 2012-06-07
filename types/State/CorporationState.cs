using System.Collections.Generic;
using System.Linq;

namespace Echo.State
{
	public class CorporationState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public IEnumerable<AgentState> Employees { get; set; }

		public CorporationState()
		{
			Employees = Enumerable.Empty<AgentState>();
		}
	}
}