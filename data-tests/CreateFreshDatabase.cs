using System;
using System.IO;
using Autofac;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Imports.Newtonsoft.Json;
using test;

namespace Echo.Data.Tests
{
	public class CreateFreshDatabase : DisposableObject
	{
		private DocumentStore _database;

		public CreateFreshDatabase()
		{
		}

		public void Create(ContainerBuilder containerBuilder)
		{
			lock ( TestSettings.SyncObject )
			{
				_database = new EmbeddableDocumentStore { RunInMemory = true, Conventions = { CustomizeJsonSerializer = CustomizeJsonSerializer } };
				_database.Initialize();

				Console.WriteLine("Using in-memory database: {0}", _database.Url);
			}

			containerBuilder.RegisterInstance(_database).As<IDocumentStore>();
		}

		private void CustomizeJsonSerializer(JsonSerializer obj)
		{
			obj.Formatting = Formatting.Indented;
			obj.TypeNameHandling = TypeNameHandling.None;
		}

		protected override void DisposeManagedResources()
		{
			if (_database != null)
			{
				_database.Dispose();
				_database = null;
			}
		}
	}
}