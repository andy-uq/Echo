using Autofac;

namespace Echo.Mapping
{
	public interface IDemandBuilder
	{
		void Build(ContainerBuilder containerBuilder);
	}
}