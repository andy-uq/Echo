using System;
using System.Collections.Generic;
using Echo.Agents.Skills;
using Echo.Statistics;

namespace Echo.State
{
	public sealed class SkillInfo : IObjectState, IObject
	{
		public SkillCode Code { get; set; }
		public AgentStatistic PrimaryStat { get; set; }
		public AgentStatistic SecondaryStat { get; set; }
		public int TrainingMultiplier { get; set; }

		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<SkillLevel> Prerequisites { get; set; }

		long IObject.Id
		{
			get { return ObjectId; }
		}

		public long ObjectId
		{
			get { return Code.ToId(); }
		}

		ObjectType IObject.ObjectType
		{
			get { return ObjectType.Skill; }
		}
	}
}