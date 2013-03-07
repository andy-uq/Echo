using System;
using System.Linq;
using System.Collections.Generic;

namespace Echo
{
	public class IdResolutionContext : IdResolver
	{
		private readonly Dictionary<long, IObject> _lookup;

		public IdResolutionContext(IEnumerable<IObject> collection)
		{
			collection = collection.ToArray();

			var q =
				(
					from o in collection
					group o by o.Id
					into g
					where g.Count() > 1
					select string.Format("{0}: {1}", g.Key, string.Join(", ", g.Select(x => x.Name)))
				).ToArray();

			if (q.Any())
				throw new InvalidOperationException("Duplicate keys detected: " + string.Join("; ", q));

			_lookup = collection.Where(x => x.Id != 0).ToDictionary(x => x.Id);
		}

		protected override IEnumerable<IObject> Values
		{
			get { return _lookup.Values.ToArray(); }
		}

		public static IIdResolver Empty
		{
			get { return new IdResolutionContext(Enumerable.Empty<IObject>()); }
		}

		protected override bool LookupValue<T>(long id, out IObject value)
		{
			return _lookup.TryGetValue(id, out value);
		}
	}
}