namespace Echo.Agents.Skills
{
	public enum SkillCode
	{
		Invalid,

		[Category(SkillCategory.SpaceshipCommand)]
		SpaceshipCommand
	}

	public static class SkillCodes
	{
		public static readonly SkillCode[] All = {SkillCode.SpaceshipCommand};
	}
}