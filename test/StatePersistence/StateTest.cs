using System;
using System.Data.SqlServerCe;
using System.Linq;
using Echo.Tests.Infrastructure;
using NUnit.Framework;
using SisoDb;
using test;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class StateTest
	{
		private string _connectionString;
		private IDisposable _databaseHandle;

		protected ISisoDatabase Database { get; set; }
		protected MockUniverse Universe { get; set; }

		[SetUp]
		public virtual void SetUp()
		{
			var fresh = new CreateFreshDatabase("starClusters");
			var configurationBuilder = new Autofac.ContainerBuilder();

			fresh.Create(configurationBuilder);
			_connectionString = fresh.ConnectionString;

			var resolver = new AutofacResolver(configurationBuilder);

			Database = resolver.Resolve<ISisoDatabase>();
			Universe = new MockUniverse();

			_databaseHandle = fresh;
		}

		[TearDown]
		public void TearDown()
		{
			if (_databaseHandle != null)
				_databaseHandle.Dispose();
		}

		protected void DumpObjects(string name)
		{
			using (var connection = new SqlCeConnection(_connectionString))
			{
				connection.Open();

				// RunQuery(connection, "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE '%Structure'");
				RunQuery(connection, string.Format("SELECT * FROM {0}StateStructure", name));
			}
		}

		private static void RunQuery(SqlCeConnection connection, string sql)
		{
			var cmd = connection.CreateCommand();
			cmd.CommandText = sql;
			using (var reader = cmd.ExecuteReader())
			{
				while ( reader.Read() )
				{
					var q =
						(
							from i in Enumerable.Range(0, reader.FieldCount)
							select new {name = reader.GetName(i), value = reader.GetValue(i)}
						).ToArray();

					foreach (var x in q)
						Console.WriteLine("{0}: {1}", x.name, x.value);
				}
			}
		}
	}
}