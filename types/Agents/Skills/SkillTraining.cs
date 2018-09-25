using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.State;

namespace Echo.Agents.Skills
{
	public class SkillTraining
	{
		public SkillTraining(SkillInfo skill)
		{
			Skill = skill;
		}

		public SkillCode SkillCode => Skill.Code;
		public SkillInfo Skill { get; }

		public long Start { get; set; }
		public long? Complete { get; set; }
		public int Remaining { get; set; }
		public bool Paused { get; set; }
	}

	public class SkillTrainingCollection : IEnumerable<SkillTraining>
	{
		private readonly List<SkillTraining> _collection = new List<SkillTraining>();

		public SkillTrainingCollection()
		{
		}

		public SkillTrainingCollection(IEnumerable<SkillTraining> training)
		{
			_collection.AddRange(training);
		}

		public SkillTraining Add(SkillTraining skill)
		{
			_collection.Add(skill);
			return skill;
		}

		public bool IsTraining(SkillCode skill)
		{
			return _collection.Any(x => x.SkillCode == skill);
		}

		public IEnumerator<SkillTraining> GetEnumerator() => _collection.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _collection).GetEnumerator();
	}
}