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
				throw new ArgumentException("Value must be null", param.Name);
			
			return param;
		}
	}
}
