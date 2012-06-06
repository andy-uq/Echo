using System;

namespace Echo.Statistics
{
	public class Math<TValue>
	{
		public delegate TValue Binary(TValue left, TValue right);

		public Math(Binary add, Predicate<TValue> isPositive, Predicate<TValue> isNegative, Predicate<TValue> isZero)
		{
			Add = add;
			IsPositive = isPositive;
			IsNegative = isNegative;
			IsZero = isZero;
		}

		public Binary Add { get; private set; }
		public Predicate<TValue> IsPositive { get; private set; }
		public Predicate<TValue> IsNegative { get; private set; }
		public Predicate<TValue> IsZero { get; private set; }
	}

	public static class Math
	{
		public static Math<int> Int
		{
			get
			{
				return new Math<int>
				(
					add: (a, b) => a + b,
					isNegative: v => v < 0,
					isPositive: v => v > 0,
					isZero: v => v == 0
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
					isZero: Units.IsZero
				);
			}
		}
	}
}