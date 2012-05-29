using System;
using System.Collections.Generic;
using Echo.JumpGates;

namespace Echo
{
	public interface IIdResolver
	{
		T GetById<T>(long id) where T : class, IObject;
	}

	public static class IdResolverExtensions
	{
		public static T Resolve<T>(this IdResolutionContext<T> context, IIdResolver register)
		{
			foreach ( var action in context.Resolved )
				action(register, context.Target);

			return context.Target;
		}
	}

	public class IdResolutionContext<T>
	{
		public IdResolutionContext()
		{
			Resolved = new List<Action<IIdResolver, T>>();
		}

		public T Target { get; set; }
		public List<Action<IIdResolver, T>> Resolved { get; set; }
	}
}