using Echo.Objects;

namespace Echo.Web.Controls.Base
{
	public interface IDashboardPage
	{
	}

	public interface IStarClusterPage
	{
		StarCluster.Template StarCluster { get; }
		SolarSystem.Template SolarSystem { get; }
		Planet.Template Planet { get; }
		Moon.Template Moon { get; }
	}

	public interface IMarketPage
	{
	}

	public interface IStructuresPage
	{
	}

	public interface IPersonnelPage
	{
	}

	public interface IShipsPage
	{
	}
}