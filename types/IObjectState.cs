namespace Echo
{
	public interface IObjectState
	{
		ObjectType ObjectType { get; }
		long Id { get; }
		string Name { get; }
	}
}