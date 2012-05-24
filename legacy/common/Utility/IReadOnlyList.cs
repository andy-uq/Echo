using System;
using System.Collections;
using System.Collections.Generic;

namespace Echo
{
	public interface IReadOnlyList<T> : IEnumerable<T>
	{
		int Count { get; }
		T this[int index] { get; }
		bool Contains(T item);
		void CopyTo(T[] array, int arrayIndex);
		int IndexOf(T item);
		List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter);
		void ForEach(Action<T> action);
		void ForEach(Predicate<T> match, Action<T> action);
		T Find(Predicate<T> match);
		List<T> FindAll(Predicate<T> match);
		List<T> GetRange(int index, int count);
	}
}