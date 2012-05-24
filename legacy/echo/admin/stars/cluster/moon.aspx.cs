using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Objects;
using Echo.Vectors;
using Echo.Web.Controls.Base;

namespace Echo.Web.Administration.Stars
{
	public partial class EditMoonPage : EchoAdminPage, IStarClusterPage
	{
		#region IStarClusterPage Members
		
		public SolarSystem.Template SolarSystem
		{
			get
			{
				return Planet.SolarSystem;
			}
		}

		public Planet.Template Planet
		{
			get
			{
				return Moon.Planet;
			}
		}

		public Moon.Template Moon
		{
			get
			{
				var templateID = ulong.Parse(Request.QueryString["t"]);
				return Global.Universe.ObjectFactory.GetTemplate<Moon.Template>(templateID);
			}
		}

		public StarCluster.Template StarCluster
		{
			get
			{
				return SolarSystem.StarCluster;
			}
		}

		#endregion

		protected void OnUpdate(object sender, EventArgs e)
		{
			foreach ( SolarSystem.Template solarsystem in StarCluster.SolarSystems )
			{
				if ( Vector.Intersects(SolarSystem.Coordinates, SolarSystem.Extent, solarsystem.Coordinates, solarsystem.Extent) == false )
					continue;

				this.badPosition.IsValid = false;
				this.badPosition.ErrorMessage = string.Format("Sorry, but this solar system would now collide with the {0} solar system", solarsystem.Name);
				return;
			}

			foreach (StarCluster.Template cluster in Global.Universe.ObjectFactory.StarClusters)
			{
				if (Vector.Intersects(StarCluster.Coordinates, StarCluster.Extent, cluster.Coordinates, cluster.Extent) == false)
					continue;

				this.badPosition.IsValid = false;
				this.badPosition.ErrorMessage = string.Format("Sorry, but this cluster would now collide with the {0} cluster", cluster.Name);
				return;
			}

			Global.Universe.ObjectFactory.StarClusters.Add(StarCluster);
			Global.SaveTemplates();

			Response.Redirect("~/admin/starclusters.aspx");
		}
	}
}