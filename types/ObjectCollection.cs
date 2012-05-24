using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Echo;
using EnsureThat;

namespace Echo
{
	[DebuggerDisplay("{Location}: {Count}")]
	public class ObjectCollection<T> : IEnumerable<T> where T : IObject
	{
		private readonly List<T> _contents = new List<T>();

		public ObjectCollection(ILocation location)
		{
			Location = location;
		}

		public ObjectCollection(ILocation location, IEnumerable<T> collection)
			: this(location)
		{
			Ensure.That(collection, "collection").IsNotNull();

			foreach (T item in collection)
				Add(item);
		}

		public ILocation Location { get; private set; }

		public T this[int index]
		{
			get { return _contents[index]; }
			set
			{
				Move(value, Location);
				_contents[index] = value;
			}
		}

		private void Move(T value, ILocation location)
		{
			
		}

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return _contents.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		public void Add(T item)
		{
			Move(item, Location);
			_contents.Add(item);
		}

		public void Insert(int index, T item)
		{
			Move(item, Location);
			_contents.Insert(index, item);
		}
	}
}