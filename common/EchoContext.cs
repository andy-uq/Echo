using Echo.Mapping;
using Echo.Ships;

namespace Echo
{
	public class EchoContext : IEchoContext
	{
		public EchoContext(ITypeMapper typeMapper, IRandom random, IIdGenerator idGenerator, IIdResolver resolver)
		{
			TypeMapper = typeMapper;
			Random = random;
			IdGenerator = idGenerator;
			Resolver = resolver;
		}

		public ITypeMapper TypeMapper { get; }
		public IRandom Random { get; }
		public IIdGenerator IdGenerator { get; }
		public IIdResolver Resolver { get; }
	}
}