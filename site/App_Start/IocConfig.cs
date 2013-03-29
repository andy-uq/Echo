using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Echo.State;
using Echo.Web.Areas.Admin.Controllers;

namespace Echo.Web
{
	public static class IocConfig
	{
		private static IContainer _container;

		public static void RegisterIoc()
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder
				.RegisterInstance(new MemoryBackingStore<StarClusterState>(new StarClusterState() { Id = "StarClusters/1", Name = "bOB"}))
				.As<IBackingStore<StarClusterState>>();
			containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
    
			_container = containerBuilder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
		}

		public static void Shutdown()
		{
			_container.Dispose();
		}
	}

	public class MemoryBackingStore<T> : IBackingStore<T>
	{
		private readonly List<T> _data;

		public MemoryBackingStore()
		{
			_data = new List<T>();
		}

		public MemoryBackingStore(params T[] initialContents)
		{
			_data = new List<T>(initialContents);
		}

		public IEnumerable<T> GetAll()
		{
			return _data;
		}

		public void Add(T value)
		{
			_data.Add(value);
		}
	}
}