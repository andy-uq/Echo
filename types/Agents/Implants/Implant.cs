using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Statistics;

namespace Echo.Agents.Implants
{
	public class Implant : IStatisticDelta<int>, IEquatable<Implant>
	{
		private static readonly AgentStatistic[] AgentStatistics = (AgentStatistic[]) Enum.GetValues(typeof (AgentStatistic));
		private static readonly Lazy<Implant[]> _defaultImplants = new Lazy<Implant[]>(() => AgentStatistics.Select(x => new Implant(x)).ToArray());

		public static IEnumerable<Implant> DefaultImplants { get { return _defaultImplants.Value; } }

		public Implant()
		{
		}

		public Implant(AgentStatistic stat)
		{
			Stat = stat;
		}

		public AgentStatistic Stat { get; set; }
		public int Rank { get; set; }
		public int Value { get; set; }

		public bool Equals(Implant other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Stat == other.Stat;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Implant) obj);
		}

		public override int GetHashCode()
		{
			return (int) Stat;
		}
	}
}