using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Statistics;

namespace Echo.Agents.Implants
{
	public class ImplantCollection : IEnumerable<Implant>
	{
		private readonly Dictionary<AgentStatistic, Implant> _implants;

		public ImplantCollection()
		{
			_implants = new Dictionary<AgentStatistic, Implant>();
		}

		public ImplantCollection(IEnumerable<State.Implant> skills)
		{
			_implants = skills.ToDictionary(s => s.Stat, s => new Implant(s));
		}

		public Implant this[AgentStatistic stat] => _implants.TryGetValue(stat, out var implant)
			? implant
			: new Implant(stat) {Rank = 0, Value = 0};

		public IEnumerator<Implant> GetEnumerator() => _implants.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}