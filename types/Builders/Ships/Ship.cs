using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Builder;
using Echo.Builders;
using Echo.State;

using ShipStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.ShipStatistic, double>;

namespace Echo.Ships
{
	partial class Ship
	{
		public static class Builder
		{
			public static ShipState Save(Ship ship)
			{
				return new ShipState
				{
					Id = ship.Id,
					Name = ship.Name,
					LocalCoordinates = ship.Position.LocalCoordinates,
		 			Statistics = ship.Statistics.Select(Save),
					HardPoints = ship.HardPoints.Save(),
					Pilot = ship.Pilot.Save(),
				};
			}

			public static ObjectBuilder<Ship> Build(ILocation location, ShipState state)
			{
				var ship = new Ship
				{
					Id = state.Id,
					Name = state.Name,
					Position = new Position(location, state.LocalCoordinates),
					Statistics = new ShipStatistics(state.Statistics.Select(Build)),
				};

				var builder = new ObjectBuilder<Ship>(ship)
					.Resolve((resolver, target) => BuildHardPoints(ship, state.HardPoints));

				builder
					.Dependent(state.Pilot)
					.Build(Agent.Builder.Build)
					.Resolve((resolver, target, dependentObject) => target.Pilot = dependentObject);

				return builder;
			}

			private static void BuildHardPoints(Ship ship, IEnumerable<HardPointState> hardPoints)
			{
				if ( hardPoints == null )
					return;
				
				ship._hardPoints.AddRange(hardPoints.Select(h => HardPoint.Builder.Build(ship, h)));
			}

			private static ShipStatisticValue Build(ShipStatisticState x)
			{
				return new ShipStatisticValue(x.Statistic, x.Value);
			}

			private static ShipStatisticState Save(ShipStatisticValue x)
			{
				return new ShipStatisticState
				{
					Statistic = x.Stat,
					CurrentValue = x.CurrentValue,
					Value = x.Value,
				};
			}
		}
	}
}