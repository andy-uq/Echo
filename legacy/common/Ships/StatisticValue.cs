using System;

namespace Echo
{
	public class StatisticValue<T, V> : IComparable<V> where V : IComparable<V>
	{
		public StatisticValue(T stat, V value)
		{
			Stat = stat;

			Value = value;
			CurrentValue = value;
		}

		public V Value { get; set; }
		public V CurrentValue { get; set; }
		public T Stat { get; private set; }

		public StatisticValue<T, V> Clone()
		{
			return (StatisticValue<T, V>)MemberwiseClone();
		}

		public void SetValue(V value)
		{
			Value = value;
			CurrentValue = value;
		}

		public void SetValue(V value, V currentValue)
		{
			Value = value;
			CurrentValue = currentValue;
		}
        
		public bool IsBuffed
		{
			get { return CurrentValue.CompareTo(Value) > 0; }
		}

		public bool IsDebuffed
		{
			get { return CurrentValue.CompareTo(Value) < 0; }
		}

		public void Alter(V buffedValue)
		{
			CurrentValue = buffedValue;
		}

		#region Operators

		public static bool operator <(StatisticValue<T, V> stat, V value)
		{
			return stat.CurrentValue.CompareTo(value) < 0;
		}

		public static bool operator >(StatisticValue<T, V> stat, V value)
		{
			return stat.CurrentValue.CompareTo(value) > 0;
		}

		public static bool operator <=(StatisticValue<T, V> stat, V value)
		{
			return stat.CurrentValue.CompareTo(value) <= 0;
		}

		public static bool operator >=(StatisticValue<T, V> stat, V value)
		{
			return stat.CurrentValue.CompareTo(value) >= 0;
		}

		#endregion

		int IComparable<V>.CompareTo(V other)
		{
			return CurrentValue.CompareTo(other);
		}
	}
}