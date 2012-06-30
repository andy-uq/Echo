using Echo.Mapping;

namespace Echo
{
	public interface IEchoContext
	{
		ITypeMapper TypeMapper { get; }
		IRandom Random { get; }
	}
}