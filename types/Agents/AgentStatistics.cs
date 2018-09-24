using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Statistics;
using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;

namespace Echo.Agents
{
	public class AgentStatistics : IEnumerable<AgentStatisticValue>
	{
		private readonly Dictionary<AgentStatistic, AgentStatisticValue> _stats;

		public AgentStatistics() : this(Enumerable.Empty<AgentStatisticValue>())
		{
		}

		public AgentStatistics(IEnumerable<AgentStatisticValue> initialStats)
		{
			_stats = new Dictionary<AgentStatistic, AgentStatisticValue>();

			var factoryStats = initialStats.ToDictionary(x => x.Stat);
			foreach ( AgentStatistic stat in Enum.GetValues(typeof(AgentStatistic)) )
			{
				var value = new AgentStatisticValue(stat, 0);
				if ( factoryStats.TryGetValue(stat, out var factoryValue) )
				{
					value = factoryValue;
				}

				_stats.Add(stat, value);
			}
		}

		public IEnumerator<AgentStatisticValue> GetEnumerator()
		{
			return _stats.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public AgentStatisticValue this[AgentStatistic stat] => _stats[stat];

		/// <summary>
		/// Charisma describes the character's attractiveness to other people and how easy going he is. High charisma helps train skills related to dealings with computer controlled characters.
		/// </summary>
		public AgentStatisticValue Charisma => this[AgentStatistic.Charisma];

		/// <summary>
		/// Intelligence describes the character's capacity for creative and logic-based thinking and reasoning. Intelligence helps in scientific pursuits and other activities requiring good deductive skills.
		/// </summary>
		public AgentStatisticValue Intelligence => this[AgentStatistic.Intelligence];

		/// <summary>
		/// Perception describes the situational awareness of the character and his ability to adapt and react quickly to a rapidly changing situation. Good perception helps the character train skills needed to fly and maneuver a spaceship and operate its weaponry.
		/// </summary>
		public AgentStatisticValue Perception => this[AgentStatistic.Perception];

		/// <summary>
		/// Memory describes the character's ability to store and retrieve data quickly and efficiently. Good memory makes complex and repetitive activities easier to perform, such as activities of manufacturing and production. Industry and Science skills use the Memory attribute.
		/// </summary>
		public AgentStatisticValue Memory => this[AgentStatistic.Memory];

		/// <summary>
		/// Willpower describes the character's drive and determination. Willpower is very useful for commanders and allows for faster training of their leadership abilities.
		/// </summary>
		public AgentStatisticValue Willpower => this[AgentStatistic.Willpower];

		public void Recalculate()
		{
			foreach ( var stat in _stats.Values )
			{
				stat.Recalculate();
			}
		}
	}
}