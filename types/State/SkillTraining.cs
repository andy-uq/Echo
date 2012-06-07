using Echo.Agents.Skills;

namespace Echo.State
{
	public class SkillTraining
	{
		public SkillCode SkillCode { get; set; }
		public long Start { get; set; }
		public long? Complete { get; set; }
		public int Remaining { get; set; }
		public bool Paused { get; set; }
	}
}