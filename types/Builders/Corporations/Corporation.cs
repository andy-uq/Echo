using System;
using System.Linq;
using Echo.Agents;
using Echo.Builder;
using Echo.State;

namespace Echo.Corporations
{
	partial class Corporation
	{
		public static class Builder
		{
			public static CorporationState Save(Corporation corporation)
			{
				return new CorporationState
				{
					ObjectId = corporation.Id,
					Name = corporation.Name,
					Employees = corporation.Employees.Select(Agent.Builder.Save),
				};
			}

			public static ObjectBuilder<Corporation> Build(CorporationState state)
			{
				var corporation = new Corporation
				{
					Id = state.ObjectId,
					Name = state.Name,
				};

				var builder = new ObjectBuilder<Corporation>(corporation);

				builder
					.Dependents(state.Employees)
					.Build(Agent.Builder.Build)
					.Resolve((resolver, target, agent) => AddEmployee(target, agent));

				return builder;
			}

			private static Agent AddEmployee(Corporation target, Agent agent)
			{
				target.Employees.Add(agent);
				agent.Corporation = target;

				return agent;
			}
		}
	}
}