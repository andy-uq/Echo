using System.Linq;
using Echo.Agents.Skills;
using Echo.Corporations;
using Echo.Items;
using Echo.State;
using Echo.Structures;
using Echo.Tasks.Agents.Train;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;
using Shouldly;
using test.common;

namespace Echo.Tests.Agents
{
	public class TrainingTests
	{
		private MockUniverse       _universe;
		private Structure          _tradingStation;

		public Structure TradingStation => _tradingStation;
		public SkillInfo Skill => _universe.SpaceshipCommand;

		[SetUp]
		public void CreateUniverse()
		{
			_universe = new MockUniverse();

			var builder = Echo.Structures.TradingStation.Builder.For(_universe.TradingStation).Build(null);
			builder.Add(Corporation.Builder.Build(_universe.MSCorp));
			
			_tradingStation = builder.Materialise();
		}

		private TrainAgentSkillTask CreateAgentTask(TrainAgentSkillParameters parameters)
		{
			var task = new TrainAgentSkillTask();
			task.SetParameters(parameters);

			return task;
		}

		[Test]
		public void TimeToTrainToLevel1()
		{
			var timeToTrain = _universe.SpaceshipCommand.GetTimeToTrainToNextLevel(0);
			timeToTrain.ShouldBe(5);
		}

		[Test]
		public void TimeToTrainToLevel30()
		{
			long timeSpentSoFar = 0;
			var lastLevel = 0;

			for (var i = 0; i < 30; i++)
			{
				var delta = _universe.SpaceshipCommand.GetTimeToTrainToNextLevel(0);
				delta.ShouldBeGreaterThanOrEqualTo(lastLevel);
				lastLevel = delta;

				timeSpentSoFar += delta;
				timeSpentSoFar.ShouldBeLessThan(int.MaxValue);
			}
		}
		
		[Test]
		public void TrainOverTime()
		{
			var corporation = Corporation.Builder.Build(_universe.MSCorp).Materialise();

			var agent = _universe.John.StandUp(corporation, initialLocation:TradingStation);

			var parameters = new TrainAgentSkillParameters
			{
				Agent = agent
			};
			
			var training = CreateAgentTask(parameters);
			var trainingTime = agent.Training.Dequeue().Remaining;

			TrainAgentSkillResult result;
			while ( trainingTime > 1 )
			{
				result = training.Train();
			
				result.Success.ShouldBeTrue();
				result.StatusCode.ShouldBe(TrainAgentSkillTask.StatusCode.Pending);

				TradingStation.Tasks.ShouldContain(training);
				trainingTime--;
			}

			result = training.Train();

			result.Success.ShouldBeTrue();
			result.StatusCode.ShouldBe(TrainAgentSkillTask.StatusCode.Success);
			
			TradingStation.Tasks.ShouldNotContain(training);
			agent.Training.ShouldBeEmpty();
		}
	}
}