using Echo.Mapping;
using Echo.Ships;

namespace Echo
{
	public class EchoContext : IEchoContext
	{
		public EchoContext(ITypeMapper typeMapper, IRandom random, IIdGenerator idGenerator)
		{
			TypeMapper = typeMapper;
			Random = random;
			IdGenerator = idGenerator;
		}

		public ITypeMapper TypeMapper { get; }
		public IRandom Random { get; }
		public IIdGenerator IdGenerator { get; }
	}
}