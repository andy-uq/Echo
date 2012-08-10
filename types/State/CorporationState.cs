using System;
using System.Collections.Generic;
using System.Linq;

namespace Echo.State
{
	public class CorporationState : IObjectState
	{
		public string Id { get; set; }
		public long ObjectId { get; set; }
		public string Name { get; set; }

		public IEnumerable<AgentState> Employees { get; set; }

		public CorporationState()
		{
			Employees = Enumerable.Empty<AgentState>();
		}
	}
}