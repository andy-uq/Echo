﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Echo
{
	public class IdResolutionContext : IdResolver
	{
		private readonly Dictionary<ulong, IObject> _lookup;

		public IdResolutionContext(IEnumerable<IObject> collection)
		{
			collection = collection.ToList();

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

		public override IEnumerable<IObject> Values
		{
			get { return _lookup.Values.ToArray(); }
		}

		protected override bool LookupValue<T>(ulong id, out IObject value)
		{
			return _lookup.TryGetValue(id, out value);
		}
	}
}