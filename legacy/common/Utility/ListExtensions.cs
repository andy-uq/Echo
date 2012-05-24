using System;
using System.Collections.Generic;

using Echo.Objects;

namespace Echo
{
	public static class ListExtensions
	{
		public static IReadOnlyList<T> ReadOnly<T>(this IEnumerable<T> list) where T : IObject
		{
			return new BaseCollection<T>(list);
		}

		public static List<T> Clone<T>(this IEnumerable<T> list) where T : ICloneable
		{
			IEnumerator<T> e = list.GetEnumerator();
			var results = new List<T>();

			while (e.MoveNext())
			{
				var clone = (T) e.Current.Clone();
				results.Add(clone);
			}

			return results;
		}
	}
}