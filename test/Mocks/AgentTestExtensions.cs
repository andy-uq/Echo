using Echo.Agents;
using Echo.State;

namespace Echo.Tests.Mocks
{
	static internal class AgentTestExtensions
	{
		public static Agent StandUp(this AgentState state, ILocation initialLocation = null)
		{
			var idResolver = IdResolver.Empty;

			if (initialLocation != null)
			{
				state.Location = initialLocation.AsObjectReference();
				idResolver = new IdResolutionContext(new[] { initialLocation });
			}

			var builder = Agent.Builder.Build(state);
			return builder.Materialise(idResolver.RegisterTestSkills());
		}
	}
}