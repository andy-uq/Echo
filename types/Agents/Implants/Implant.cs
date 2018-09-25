using System;
using Echo.Statistics;

namespace Echo.Agents.Implants
{
	public class Implant : IStatisticDelta<int>, IEquatable<Implant>
	{
		public Implant(AgentStatistic stat)
		{
			Stat = stat;
		}

		public AgentStatistic Stat { get; }
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
			return obj.GetType() == GetType() && Equals((Implant) obj);
		}

		public override int GetHashCode() => (int) Stat;
	}
}