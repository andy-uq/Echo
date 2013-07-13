using Echo.Agents;
using Echo.Corporations;
using Echo.State;
using test.common;

namespace Echo.Tests.Mocks
{
	static internal class AgentTestExtensions
	{
		public static Agent StandUp(this AgentState state, Corporation corporation = null, ILocation initialLocation = null)
		{
			var idResolver = IdResolver.Empty;

			if (initialLocation != null)
			{
				state.Location = initialLocation.AsObjectReference();
				idResolver = new IdResolutionContext(new[] { initialLocation });
			}

			var builder = Agent.Builder.Build(state)
				.Resolve((r, a) => a.Corporation = corporation);
			
			return builder.Materialise(idResolver.RegisterTestSkills());
		}
	}
}