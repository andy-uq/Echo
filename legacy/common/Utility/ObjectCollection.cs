using System;
using System.Collections.Generic;
using System.Diagnostics;

using Echo.Objects;

using Ubiquity.u2ool.Collections;

namespace Echo
{
	[DebuggerDisplay("{Location}: {Count}")]
	public class ObjectCollection<T> : BaseCollection<T> where T : IObject
	{
		public ILocation Location { get; private set; }

		public ObjectCollection(ILocation location)
		{
			if (location == null)
				throw new ArgumentNullException("location");

			Location = location;
		}

		public ObjectCollection(ILocation location, IEnumerable<T> collection) : this(location)
		{
			ListUtility.ForEach(collection, Add);
		}

		public override void Add(T item)
		{
			item.Location = Location;
			base.Add(item);
		}

		public override void Insert(int index, T item)
		{
			item.Location = Location;
			base.Insert(index, item);
		}

		public override T this[int index]
		{
			get { return base[index]; }
			set
			{
				value.Location = Location;
				base[index] = value;
			}
		}
	}
}