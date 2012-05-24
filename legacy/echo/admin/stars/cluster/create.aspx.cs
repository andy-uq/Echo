using System;

using Echo.Objects;
using Echo.Vectors;
using Echo.Web.Controls.Base;

namespace Echo.Web.Administration.Stars
{
	public partial class CreateStarClusterPage : EchoAdminPage, IStarClusterPage
	{
		#region IStarClusterPage Members

		public StarCluster.Template StarCluster
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

		protected void OnCreate(object sender, EventArgs e)
		{
			var starcluster = new StarCluster.Template(Global.Universe.ObjectFactory);
			string name = this.txtName.Text.Trim();

			if (name.Length > 0)
				starcluster.Name = name;

			double x = double.Parse(this.txtX.Text);
			double y = double.Parse(this.txtY.Text);

			starcluster.Coordinates = new Vector(x, y, 0d);

			Global.Universe.ObjectFactory.StarClusters.Add(starcluster);
			Global.Universe.ObjectFactory.Register(starcluster);
			Global.SaveTemplates();

			Response.Redirect("~/admin/starclusters.aspx");
		}
	}
}