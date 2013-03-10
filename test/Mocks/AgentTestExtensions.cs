using Echo.Agents;
using Echo.State;

namespace Echo.Tests.Mocks
{
	static internal class AgentTestExtensions
	{
		public static Agent StandUp(this AgentState state)
		{
			var builder = Agent.Builder.Build(state);
			builder.RegisterTestSkills();
			return builder.Materialise();
		}
	}
}