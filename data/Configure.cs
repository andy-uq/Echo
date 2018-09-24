using Autofac;
using Autofac.Core;
using Echo.Mapping;
using Newtonsoft.Json;
using Raven.Client.Documents;

namespace Echo.Data
{
	public class Configure : IDemandBuilder
	{
		public void Build(ContainerBuilder containerBuilder)
		{
			containerBuilder.RegisterType<DocumentStore>()
				.As<IDocumentStore>()
				.OnActivated(InitialiseDocumentStore)
				.SingleInstance();
		}

		private void InitialiseDocumentStore(IActivatedEventArgs<DocumentStore> obj)
		{
			obj.Instance.Urls = new[] {"http://localhost:8080"};
			obj.Instance.Conventions.CustomizeJsonSerializer = ConfigureJsonSerialiser;
			obj.Instance.Initialize();
		}

		private void ConfigureJsonSerialiser(JsonSerializer obj)
		{
			obj.TypeNameHandling = TypeNameHandling.None;
		}
	}
}