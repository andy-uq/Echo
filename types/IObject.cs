using Echo;

namespace Echo
{
	public interface IObject
	{
		ObjectType ObjectType { get; }
		long Id { get; }
		string Name { get; }

		void Tick(ulong tick);
	}
}