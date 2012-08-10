using System;
using System.IO;
using Autofac;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using test;

namespace Echo.Tests.Infrastructure
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
				_database = new EmbeddableDocumentStore { RunInMemory = true };
				_database.Initialize();

				Console.WriteLine("Using in-memory database: {0}", _database.Url);
			}

			containerBuilder.RegisterInstance(_database).As<IDocumentStore>();
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