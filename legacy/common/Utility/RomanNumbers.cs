using System;

namespace Echo
{
	public static class RomanNumbers
	{
		public static string ConvertToRomanNumerals(int n)
		{
			// converts number 1 - 4999 to a subtractive Roman numeral ...
			return (Cvt1000(n) + Cvt100s(n) + Cvt10s(n) + Cvt1s(n));
		}

		private static string Cvt1000(int n)
		{
			// converts thousands digit to a Roman numeral ...
			n = (int)Math.Floor(n / 1000M);
			return "mmmm".Substring(0, n);
		}

		private static string Cvt100s(int h)
		{
			// converts hundreds digit to a subtractive Roman numeral ...
			h = (int)Math.Floor((h % 1000M) / 100M);
			if ( h == 9 )
			{
				return "cm";
			}
			if ( h > 4 )
			{
				return "dccc".Substring(0, h - 4);
			}
			if ( h == 4 )
			{
				return "cd";
			}
			return "ccc".Substring(0, h);
		}

		private static string Cvt10s(int t)
		{
			// converts tens digit to a subtractive Roman numeral ...
			t = (int)Math.Floor((t % 100M) / 10M);
			if ( t == 9 )
			{
				return "xc";
			}
			if ( t > 4 )
			{
				return "lxxx".Substring(0, t - 4);
			}
			if ( t == 4 )
			{
				return "xl";
			}
			return "xxx".Substring(0, t);
		}

		private static string Cvt1s(int u)
		{
			// converts units digit to a subtractive Roman numeral ...
			u = u % 10;
			if ( u == 9 )
			{
				return "ix";
			}
			if ( u > 4 )
			{
				return "viii".Substring(0, u - 4);
			}
			if ( u == 4 )
			{
				return "iv";
			}
			return "iii".Substring(0, u);
		}
	}
}
