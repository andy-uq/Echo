using System;
using System.Threading;
using Autofac;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Embedded;

namespace Echo.Tests
{
	public class CreateFreshDatabase : DisposableObject
	{
		private static readonly Lazy<EmbeddedServer> s_database = new Lazy<EmbeddedServer>(() =>
		{
			var database = EmbeddedServer.Instance;
			database.StartServer();

			Console.WriteLine($"Using in-memory database: {database.GetServerUriAsync().GetAwaiter().GetResult()}");
			return database;
		});

		private static int _dbId = 1;

		public void Create(ContainerBuilder containerBuilder)
		{
			var dbId = Interlocked.Increment(ref _dbId);
			containerBuilder.RegisterInstance(s_database.Value.GetDocumentStore($"Echo_{dbId:d5}")).As<IDocumentStore>();
		}

		private void CustomizeJsonSerializer(JsonSerializer obj)
		{
			obj.Formatting = Formatting.Indented;
			obj.TypeNameHandling = TypeNameHandling.None;
		}

		protected override void DisposeManagedResources()
		{
			
		}
	}
}