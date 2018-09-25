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
		public int TrainingMultiplier { get; set; } = 1;

		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<SkillLevel> Prerequisites { get; set; } = new SkillLevel[0];

		ulong IObject.Id => ObjectId;
		public ulong ObjectId => Code.ToId();
		ObjectType IObject.ObjectType => ObjectType.Skill;

		public int GetTimeToTrainToNextLevel(int currentLevel)
		{
			var timeRequired = TimeSpentGettingToLevel(currentLevel);
			return TimeToTrainToLevel(timeRequired, currentLevel + 1);
		}

		private static int TimeSpentGettingToLevel(int currentLevel)
		{
			if (currentLevel == 0)
				return 0;

			var timeTakenSoFar = 0;
			var timeRequired = 5;

			for (var i = 2; i <= currentLevel; i++)
			{
				timeRequired = TimeToTrainToLevel(timeTakenSoFar, i);
				timeTakenSoFar += timeRequired;
			}

			return timeRequired;
		}

		private static int TimeToTrainToLevel(int timeTakenSoFar, int targetLevel)
		{
			if (timeTakenSoFar == 0)
			{
				return 5;
			}

			var multiplier = 2.0 - (1.0 / targetLevel);
			return ((int) System.Math.Ceiling(timeTakenSoFar * multiplier / 5.0) * 5) - timeTakenSoFar;
		}
	}
}