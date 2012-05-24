using Echo;

namespace Echo
{
	public interface ILocation : IObject
	{
		Position Position { get; }
	}
}