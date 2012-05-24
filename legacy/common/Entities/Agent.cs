using Echo.Objects;

namespace Echo.Entities
{
	public partial class Agent : BaseObject
	{
		public Corporation Employer { get; private set; }

		public override ObjectType ObjectType
		{
			get { return ObjectType.Agent; }
		}

		public Agent()
		{
		}

		public Agent(Corporation employer)
		{
			Employer = employer;
			Stats = new AgentStatistics();
		}

		public AgentStatistics Stats { get; private set; }

		public override string ToString()
		{
			return string.Format("{0} of {1}", Name, Employer.Name);
		}
	}
}