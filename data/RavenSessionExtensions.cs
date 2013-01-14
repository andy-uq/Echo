using System;
using System.Collections.Generic;
using Raven.Client;

namespace Echo.Data
{
	public static class RavenSessionExtensions
	{
		public static void StoreMany<T>(this IDocumentSession session, IEnumerable<T> values, string name = null, Func<T, object> idSelector = null) where T : IObjectState
		{
			if (idSelector == null)
			{
				idSelector = arg => arg.ObjectId;
			}

			foreach (var i in values)
			{
				var id = idSelector(i);
				session.Store(i, string.Concat(name ?? typeof(T).Name, '/', id));
			}
		}
	}
}