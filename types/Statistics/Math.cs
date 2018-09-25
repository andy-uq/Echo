using System;

namespace Echo.Statistics
{
	public class Math<TValue>
	{
		public delegate TValue Binary(TValue left, TValue right);
		public delegate bool Compare(TValue left, TValue right);

		public Math(Binary add, Predicate<TValue> isPositive, Predicate<TValue> isNegative, Predicate<TValue> isZero, Compare isEqual)
		{
			Add = add;
			IsPositive = isPositive;
			IsNegative = isNegative;
			IsZero = isZero;
			IsEqual = isEqual;
		}

		public Binary Add { get; }
		public Predicate<TValue> IsPositive { get; }
		public Predicate<TValue> IsNegative { get; }
		public Predicate<TValue> IsZero { get; }
		public Compare IsEqual { get; }
	}

	public static class Math
	{
		public static Math<int> Int32
		{
			get
			{
				return new Math<int>
				(
					add: (a, b) => a + b,
					isNegative: v => v < 0,
					isPositive: v => v > 0,
					isZero: v => v == 0,
					isEqual: (a, b) => a == b
				);
			}
		}

		public static Math<double> Double
		{
			get
			{
				return new Math<double>
				(
					add: (a, b) => a + b,
					isNegative: v => v < 0,
					isPositive: v => v > 0,
					isZero: Units.IsZero,
					isEqual: Units.Equal
				);
			}
		}
	}
}