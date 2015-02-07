using System;
using System.Diagnostics;
using System.Linq.Expressions;
using EnsureThat;

namespace Echo
{
	public static class EnsureObjectExtensions
	{
		public static Param<T> That<T>(Expression<Func<T>> expression)
		{
			var memberExpression = GetRightMostMember(expression);

			return new Param<T>(
				ToPath(memberExpression),
				expression.Compile().Invoke());
		}

		[DebuggerStepThrough]
		public static Param<T> IsNull<T>(this Param<T> param) where T : class
		{
			if (param.Value != null)
				throw new ArgumentException(param.Name + " must be null", param.Name);

			return param;
		}

		[DebuggerStepThrough]
		public static Param<T> IsNotEqualTo<T>(this Param<T> param, T value, string name = null)
		{
			if (Equals(param.Value, value))
				throw new ArgumentException(param.Name + " must not equal " + value, name ?? param.Name);

			return param;
		}

		[DebuggerStepThrough]
		public static Param<ulong> IsGt(this Param<ulong> param, ulong value, string name = null)
		{
			if (Equals(param.Value, value))
				throw new ArgumentException(param.Name + " must be greater than " + Convert.ToString(value),
					name ?? param.Name);

			return param;
		}

		private static string ToPath(MemberExpression e)
		{
			var path = "";
			var parent = e.Expression as MemberExpression;

			if (parent != null)
				path = ToPath(parent) + ".";

			return path + e.Member.Name;
		}

		private static MemberExpression GetRightMostMember(Expression e)
		{
			if (e is LambdaExpression)
				return GetRightMostMember(((LambdaExpression) e).Body);

			if (e is MemberExpression)
				return (MemberExpression) e;

			if (e is MethodCallExpression)
			{
				var callExpression = (MethodCallExpression) e;

				if (callExpression.Object is MethodCallExpression || callExpression.Object is MemberExpression)
					return GetRightMostMember(callExpression.Object);

				var member = callExpression.Arguments.Count > 0 ? callExpression.Arguments[0] : callExpression.Object;
				return GetRightMostMember(member);
			}

			if (e is UnaryExpression)
			{
				var unaryExpression = (UnaryExpression) e;
				return GetRightMostMember(unaryExpression.Operand);
			}

			return null;
		}
	}
}
