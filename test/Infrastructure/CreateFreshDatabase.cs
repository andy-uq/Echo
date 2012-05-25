using System.IO;
using Autofac;
using SisoDb;
using SisoDb.SqlCe4;
using test;

namespace Echo.Tests.Infrastructure
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
			DbFilename = TestSettings.UniqueFileName(string.Concat(templateName, ".sdf"));
			ConnectionString = string.Format(@"data source={0}", DbFilename);
		}

		public void Create(ContainerBuilder containerBuilder)
		{
			_database = new SqlCe4Database(new SqlCe4ConnectionInfo(ConnectionString), new SqlCe4ProviderFactory());
			_database.EnsureNewDatabase();

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