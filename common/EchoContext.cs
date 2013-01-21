using Echo;
using Echo.Mapping;

namespace Echo
{
	public class EchoContext : IEchoContext
	{
		private readonly ITypeMapper _typeMapper;
		private readonly IRandom _random;
		private readonly IIdGenerator _idGenerator;

		public EchoContext(ITypeMapper typeMapper, IRandom random, IIdGenerator idGenerator)
		{
			_typeMapper = typeMapper;
			_random = random;
			_idGenerator = idGenerator;
		}

		public ITypeMapper TypeMapper
		{
			get { return _typeMapper; }
		}

		public IRandom Random
		{
			get { return _random; }
		}

		public IIdGenerator IdGenerator
		{
			get { return _idGenerator; }
		}
	}
}