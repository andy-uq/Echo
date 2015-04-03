using System.Collections.Generic;
using System.Linq;

namespace Echo
{
	public class CompositeIdResolver : IdResolver
	{
		private readonly IIdResolver[] _resolvers;

		public CompositeIdResolver(params IIdResolver[] resolvers)
		{
			_resolvers = resolvers;
		}

		public override IEnumerable<IObject> Values
		{
			get { return _resolvers.SelectMany(x => x.Values); }
		}

		protected override bool LookupValue<T>(ulong id, out IObject value)
		{
			foreach (var child in _resolvers)
			{
				if ( child.TryGetById(id, out value) )
					return true;
			}

			value = null;
			return false;
		}
	}
}