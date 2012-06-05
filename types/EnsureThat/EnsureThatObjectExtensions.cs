using System;
using System.Diagnostics;
using EnsureThat;

namespace Echo
{
	public static class EnsureObjectExtensions
	{
		[DebuggerStepThrough]
		public static Param<T> IsNull<T>(this Param<T> param) where T : class
		{
			if ( param.Value != null )
				throw new ArgumentException(param.Name + " must be null", param.Name);
			
			return param;
		}

		[DebuggerStepThrough]
		public static Param<T> IsNotEqualTo<T>(this Param<T> param, T value, string name = null)
		{
			if ( Equals(param.Value, value) )
				throw new ArgumentException(param.Name + " must not equal " + (name ?? value.ToString()), param.Name);
			
			return param;
		}
	}
}
