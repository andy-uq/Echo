﻿using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Statistics;

using AgentStatisticValue = Echo.Statistics.StatisticValue<Echo.Statistics.AgentStatistic, int>;

namespace Echo.State
{
	public class AgentState : IObjectState
	{
		public long ObjectId { get; set; }
		public string Name { get; set; }
		public IEnumerable<AgentStatisticState> Statistics { get; set; }		
		public IEnumerable<Implant> Implants { get; set; }
		public IEnumerable<SkillLevel> Skills { get; set; }
		public IEnumerable<SkillTraining> Training { get; set; }
	}

	public class AgentStatisticState : StatisticState<int>
	{
		public AgentStatistic Statistic { get; set; }
	}
}