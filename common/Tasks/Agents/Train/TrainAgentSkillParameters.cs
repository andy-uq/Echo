using Echo.Agents;

namespace Echo.Tasks.Agents.Train
{
	public class TrainAgentSkillParameters : ITaskParameters
	{
		public Agent Agent { get; set; }
	}
}