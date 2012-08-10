using Autofac;
using Autofac.Core;
using Echo.Mapping;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

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
			obj.Instance.Url = "http://raven.local";
			obj.Instance.DefaultDatabase = "Echo";
			obj.Instance.Conventions.CustomizeJsonSerializer = ConfigureJsonSerialiser;
			obj.Instance.Initialize();
		}

		private void ConfigureJsonSerialiser(JsonSerializer obj)
		{
			obj.TypeNameHandling = TypeNameHandling.None;
		}
	}
}