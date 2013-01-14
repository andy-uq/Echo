using System;
using System.IO;
using Autofac;
using NUnit.Framework;
using SisoDb;
using SisoDb.SqlCe4;
using test;

namespace Echo.Data.Tests
{
	public class CreateFreshDatabase : DisposableObject
	{
		public string TemplateName { get; private set; }
		public string DbFilename { get; private set; }
		public string ConnectionString { get; private set; }

		private SqlCe4Database _database;

		public CreateFreshDatabase(string templateName)
		{
			TemplateName = templateName;
		}

		public void Create(ContainerBuilder containerBuilder)
		{
			lock ( TestSettings.SyncObject )
			{
				DbFilename = TestSettings.UniqueFileName(string.Concat(TemplateName, ".sdf"));
				ConnectionString = string.Format(@"data source={0}", DbFilename);
				_database = new SqlCe4Database(new SqlCe4ConnectionInfo(ConnectionString), new SqlCe4ProviderFactory());
				_database.EnsureNewDatabase();

				Assert.That(File.Exists(DbFilename), Is.True);
				Console.WriteLine("Using database {0}", DbFilename);
			}

			containerBuilder.RegisterInstance(_database).As<ISisoDatabase>();
		}

		protected override void DisposeManagedResources()
		{
			if (_database != null)
			{
				_database.DeleteIfExists();
			}
		}
	}
}