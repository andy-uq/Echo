using Echo;
using Echo.Mapping;

namespace Echo
{
	public class EchoContext : IEchoContext
	{
		private readonly ITypeMapper _typeMapper;
		private readonly IRandom _random;

		public EchoContext(ITypeMapper typeMapper, IRandom random)
		{
			_typeMapper = typeMapper;
			_random = random;
		}

		public ITypeMapper TypeMapper
		{
			get { return _typeMapper; }
		}

		public IRandom Random
		{
			get { return _random; }
		}
	}
}