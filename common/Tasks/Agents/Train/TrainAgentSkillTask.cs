using System;
using Echo.Agents;
using Echo.Agents.Skills;
using Echo.Tasks.Structure;

namespace Echo.Tasks.Agents.Train
{
	public class TrainAgentSkillTask : ITask
	{
		public ulong Id { get; set; }
		public string Name { get; set; }
		public ObjectType ObjectType => ObjectType.Task;

		public Agent Agent { get; private set; }
		public SkillTraining Skill { get; private set; }
		public Structures.Structure Location => Agent.Location as Structures.Structure;

		public enum StatusCode
		{
			InvalidLocation,
			Pending,
			Success
		}

		public ITaskResult SetParameters(ITaskParameters taskParameters)
		{
			return SetParameters(taskParameters as TrainAgentSkillParameters);
		}

		private TrainAgentSkillResult SetParameters(TrainAgentSkillParameters parameters)
		{
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));

			Agent = parameters.Agent;

			var result = ValidateParameters();
			if (result.Success)
			{
				Skill = Agent.Training.Dequeue();
				return new TrainAgentSkillResult { Success = true, TimeRemaining = Skill.Remaining };
			}

			return result;
		}

		private TrainAgentSkillResult ValidateParameters()
		{
			if (Location == null)
			{
				return Failed(StatusCode.InvalidLocation);
			}

			return Success();
		}

		ITaskResult ITask.Execute()
		{
			return Train();
		}

		public TrainAgentSkillResult Train()
		{
			var result = ValidateParameters();
			if (!result.Success) 
				return result;

			if (!Skill.Paused)
			{
				Skill.Remaining--;
			}

			if (Skill.Remaining > 0)
			{
				Location.RegisterTask(this);
				return new TrainAgentSkillResult(StatusCode.Pending) {Success = true, TimeRemaining = Skill.Remaining};
			}

			Location.TaskComplete(this);
			Agent.Training.Remove(Skill);
			Agent.Skills[Skill.SkillCode].Level++;

			return Success();
		}

		private TrainAgentSkillResult Success() => new TrainAgentSkillResult(StatusCode.Success) { Success = true };
		private TrainAgentSkillResult Failed(StatusCode statusCode) => new TrainAgentSkillResult(statusCode);
	}
}