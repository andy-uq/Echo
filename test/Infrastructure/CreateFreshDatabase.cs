using Autofac;
using SisoDb;
using SisoDb.SqlCe4;
using test;

namespace Echo.Tests.Infrastructure
{
	public class CreateFreshDatabase
	{
		public string TemplateName { get; private set; }
		public string DbFilename { get; private set; }
		public string ConnectionString { get; private set; }

		public CreateFreshDatabase(string templateName)
		{
			TemplateName = templateName;
			DbFilename = TestSettings.UniqueFileName(string.Concat(templateName, ".sdf"));
			ConnectionString = string.Format(@"data source={0}", DbFilename);
		}

		public void Create(ContainerBuilder containerBuilder)
		{
			var database = new SqlCe4Database(new SqlCe4ConnectionInfo(ConnectionString), new SqlCe4ProviderFactory());
			database.EnsureNewDatabase();

			containerBuilder.RegisterInstance(database).As<ISisoDatabase>();
		}
	}
}