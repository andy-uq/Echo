using Echo.Mapping;

namespace Echo
{
	public interface IEchoContext
	{
		IIdGenerator IdGenerator { get; }
		ITypeMapper TypeMapper { get; }
		IRandom Random { get; }
	}
}