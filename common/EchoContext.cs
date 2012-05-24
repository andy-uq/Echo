using Echo;
using Echo.Mapping;

namespace Echo
{
	public class EchoContext : IEchoContext
	{
		private readonly ITypeMapper _typeMapper;

		public EchoContext(ITypeMapper typeMapper)
		{
			_typeMapper = typeMapper;
		}

		public ITypeMapper TypeMapper
		{
			get { return _typeMapper; }
		}
	}
}