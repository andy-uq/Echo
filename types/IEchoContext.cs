using Echo.Mapping;
using Echo.Ships;

namespace Echo
{
	public interface IEchoContext
	{
		IIdGenerator IdGenerator { get; }
		ITypeMapper TypeMapper { get; }
		IRandom Random { get; }
	}
}