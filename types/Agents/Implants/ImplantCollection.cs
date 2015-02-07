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

		public ImplantCollection(IEnumerable<Implant> skills)
		{
			_implants = skills.ToDictionary(s => s.Stat);
		}

		public Implant this[AgentStatistic stat]
		{
			get
			{
				Implant implant;
				if (_implants.TryGetValue(stat, out implant))
				{
					return implant;
				}

				return new Implant {Stat = stat, Rank = 0, Value = 0};
			}
		}

		public IEnumerator<Implant> GetEnumerator()
		{
			return _implants.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}