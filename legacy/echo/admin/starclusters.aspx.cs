using System.Collections.Generic;
using System.Web.UI.WebControls;

using Echo.Objects;
using Echo.Web.Controls.Base;

namespace Echo.Web.Administration
{
	public partial class StarClustersPage : EchoAdminPage, IStarClusterPage
	{
		protected static List<StarCluster.Template> StarClusters
		{
			get { return Global.Universe.ObjectFactory.StarClusters; }
		}

		#region IStarClusterPage Members

		StarCluster.Template IStarClusterPage.StarCluster
		{
			get { return null; }
		}

		SolarSystem.Template IStarClusterPage.SolarSystem
		{
			get { return null; }
		}

		Planet.Template IStarClusterPage.Planet
		{
			get { return null; }
		}

		Moon.Template IStarClusterPage.Moon
		{
			get { return null; }
		}

		#endregion

		protected void OnItemCommand(object source, RepeaterCommandEventArgs e)
		{
			var url = string.Format("~/admin/stars/cluster/edit.aspx?t={0}", e.CommandArgument);
			Response.Redirect(url);
		}
	}
}