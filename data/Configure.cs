using Autofac;
using SisoDb;
using SisoDb.Configurations;
using SisoDb.SqlCe4;
using Echo.Mapping;

namespace Echo.Data
{
	public class Configure : IDemandBuilder
	{
		public void Build(ContainerBuilder containerBuilder)
		{
			var connectionInfo = new SqlCe4ConnectionInfo(ConnectionString.Get("siso"));
			containerBuilder.RegisterInstance(connectionInfo).As<ISisoConnectionInfo>();
			containerBuilder.RegisterType<SqlCe4ProviderFactory>().As<IDbProviderFactory>();
			containerBuilder.RegisterType<SqlCe4Database>().As<ISisoDatabase>().SingleInstance();
		}
	}
}