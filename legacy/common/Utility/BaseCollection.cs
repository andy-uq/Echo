using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Echo.Objects;

using Ubiquity.u2ool.Collections;

namespace Echo
{
	[DebuggerDisplay("Count: {Count}")]
	public class BaseCollection<T> : IList<T>, IReadOnlyList<T> where T : IObject
	{
		private readonly List<T> contents;

		public BaseCollection()
		{
			this.contents = new List<T>();
		}

		public BaseCollection(IEnumerable<T> collection)
			: this()
		{
			ListUtility.ForEach(collection, Add);
		}

		public int Count
		{
			get { return this.contents.Count; }
		}

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.contents.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual void Add(T item)
		{
			this.contents.Add(item);
		}

		public void Clear()
		{
			this.contents.Clear();
		}

		public bool Contains(T item)
		{
			return this.contents.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.contents.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return this.contents.Remove(item);
		}

		public int IndexOf(T item)
		{
			return this.contents.IndexOf(item);
		}

		public virtual void Insert(int index, T item)
		{
			this.contents.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.contents.RemoveAt(index);
		}

		public virtual T this[int index]
		{
			get { return this.contents[index]; }
			set
			{
				this.contents[index] = value;
			}
		}

		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			return this.contents.ConvertAll(converter);
		}

		public void ForEach(Action<T> action)
		{
			this.contents.ForEach(action);
		}

		public void ForEach(Predicate<T> match, Action<T> action)
		{
			IEnumerator<T> e = GetEnumerator();
			while (e.MoveNext())
			{
				T current = e.Current;

				if (match(current))
					action(current);
			}
		}

		public T Find(Predicate<T> match)
		{
			return this.contents.Find(match);
		}

		public List<T> FindAll(Predicate<T> match)
		{
			return this.contents.FindAll(match);
		}

		public List<T> GetRange(int index, int count)
		{
			return this.contents.GetRange(index, count);
		}

		public bool Exists(Predicate<T> match)
		{
			return this.contents.Exists(match);
		}

		public int RemoveAll(Predicate<T> match)
		{
			return this.contents.RemoveAll(match);
		}

		public void AddRange(IEnumerable<T> collection)
		{
			ListUtility.ForEach(collection, Add);
		}

		public void Sort(Comparison<T> comparison)
		{
			this.contents.Sort(comparison);
		}

		public override string ToString()
		{
			bool trim = (Count > 3);

			List<T> list = trim ? this.contents.GetRange(0, 3) : this.contents;
			string result = string.Join(", ", list.ConvertAll(t => t.Name).ToArray());

			return trim ? string.Concat(result, ", ...") : result;
		}
	}
}