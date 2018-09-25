using Echo.Agents.Skills;

namespace Echo.State
{
	public class SkillTraining
	{
		public SkillTraining()
		{
		}

		public SkillTraining(SkillCode skillCode)
		{
			SkillCode = skillCode;
		}

		public SkillCode SkillCode { get; set; }
		public int Remaining { get; set; }
		public bool Paused { get; set; }
	}
}