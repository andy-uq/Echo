using Echo.Agents;
using Echo.State;

namespace Echo.Tests.Mocks
{
	static internal class AgentTestExtensions
	{
		public static Agent StandUp(this AgentState state, ILocation initialLocation = null)
		{
			if (initialLocation != null)
				state.Location = initialLocation.AsObjectReference();

			var builder = Agent.Builder.Build(state);
			builder.RegisterTestSkills();

			return builder.Materialise();
		}
	}
}