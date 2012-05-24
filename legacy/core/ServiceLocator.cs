using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Syntax;

namespace core
{
	public static class ServiceLocator
	{
		private static readonly StandardKernel Ninject = new StandardKernel();

		public static IBindingToSyntax<T> Bind<T>()
		{
			return Ninject.Bind<T>();
		}

		public static T Get<T>()
		{
			return Ninject.Get<T>();
		}
	}
}