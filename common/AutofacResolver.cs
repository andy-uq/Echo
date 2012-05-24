using Autofac;
using Echo.Mapping;

namespace Echo
{
	public class AutofacResolver : IDependencyResolver
	{
		private readonly IContainer _autoFacContainer;

		public AutofacResolver(IContainer autoFacContainer)
		{
			_autoFacContainer = autoFacContainer;
		}

		public AutofacResolver(ContainerBuilder autoFacContainer)
		{
			_autoFacContainer = autoFacContainer.Build();
		}

		public T Resolve<T>()
		{
			return _autoFacContainer.Resolve<T>();
		}

		public bool TryResolve<T>(out T value)
		{
			return _autoFacContainer.TryResolve(out value);
		}
	}
}