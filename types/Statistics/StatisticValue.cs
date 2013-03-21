using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Echo.Statistics
{
	public class StatisticValue<T, TValue> : IComparable<TValue> where TValue : IComparable<TValue>
	{
		private readonly HashSet<IStatisticDelta<TValue>> _deltas;

		private static Math<TValue> Math { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
		static StatisticValue()
		{
			var type = typeof (Math);
			var member = type.GetProperty(typeof (TValue).Name, BindingFlags.Public | BindingFlags.Static);
			if (member == null)
			{
				throw new InvalidOperationException("Cannot find Math module for type " + typeof(TValue).Name);
			}

			Math = (Math<TValue>) member.GetValue(null, null);
		}

		public StatisticValue(T stat, TValue value) : this(stat, value, Enumerable.Empty<IStatisticDelta<TValue>>())
		{
		}

		public StatisticValue(T stat, TValue value, IEnumerable<IStatisticDelta<TValue>> deltas)
		{
			Stat = stat;
			Value = value;

			_deltas = new HashSet<IStatisticDelta<TValue>>(deltas);
			Recalculate();
		}

		public void Recalculate()
		{
			CurrentValue = _deltas.Aggregate(Value, (x, delta) => Math.Add(x, delta.Value));
		}

		public TValue Value { get; set; }
		public TValue CurrentValue { get; set; }
		public T Stat { get; private set; }

		public IEnumerable<IStatisticDelta<TValue>> Buffs { get { return _deltas.Where(x => Math.IsPositive(x.Value)); } }
		public IEnumerable<IStatisticDelta<TValue>> Debuffs { get { return _deltas.Where(x => Math.IsNegative(x.Value)); } }

		public StatisticValue<T, TValue> Clone()
		{
			return (StatisticValue<T, TValue>)MemberwiseClone();
		}

		public void SetValue(TValue value)
		{
			Value = value;
			CurrentValue = value;
		}

		public void SetValue(TValue value, TValue currentValue)
		{
			Value = value;
			CurrentValue = currentValue;
		}

		public void Alter(IStatisticDelta<TValue> delta)
		{
			_deltas.Add(delta);
			Recalculate();
		}

		public bool Remove(IStatisticDelta<TValue> delta)
		{
			return _deltas.Remove(delta);
		}

		#region Operators

		public static bool operator <(StatisticValue<T, TValue> stat, TValue value)
		{
			return stat.CurrentValue.CompareTo(value) < 0;
		}

		public static bool operator >(StatisticValue<T, TValue> stat, TValue value)
		{
			return stat.CurrentValue.CompareTo(value) > 0;
		}

		public static bool operator <=(StatisticValue<T, TValue> stat, TValue value)
		{
			return stat.CurrentValue.CompareTo(value) <= 0;
		}

		public static bool operator >=(StatisticValue<T, TValue> stat, TValue value)
		{
			return stat.CurrentValue.CompareTo(value) >= 0;
		}

		#endregion

		public int CompareTo(TValue other)
		{
			return CurrentValue.CompareTo(other);
		}
	}
}