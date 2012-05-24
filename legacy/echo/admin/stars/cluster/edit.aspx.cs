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
	public partial class EditStarClusterPage : EchoAdminPage, IStarClusterPage
	{
		private StarCluster.Template _starCluster;
		private string cacheKey;
		private int planetIndex = -1;
		private int solarSystemIndex = -1;
		private int moonIndex = -1;

		public SolarSystem.Template SolarSystem
		{
			get
			{
				if (this.solarSystemIndex == -1)
					return null;

				if (StarCluster == null)
					return null;

				return StarCluster.SolarSystems[this.solarSystemIndex];
			}
		}

		public Planet.Template Planet
		{
			get
			{
				if (this.planetIndex == -1)
					return null;

				if (SolarSystem == null)
					return null;

				return SolarSystem.Planets[this.planetIndex];
			}
		}

		public Moon.Template Moon
		{
			get
			{
				if ( this.moonIndex == -1 )
					return null;

				if ( Planet == null )
					return null;

				return Planet.Moons[this.moonIndex];
			}
		}

		protected List<Planet.Template> Planets
		{
			get
			{
				if (SolarSystem == null)
					return null;

				return SolarSystem.Planets;
			}
		}

		protected List<Moon.Template> Moons
		{
			get
			{
				if ( Planet == null )
					return null;

				return Planet.Moons;
			}
		}

		#region IStarClusterPage Members

		public StarCluster.Template StarCluster
		{
			get
			{
				if (this._starCluster == null)
					this._starCluster = (StarCluster.Template) Cache[this.cacheKey];

				return this._starCluster;
			}
		}

		#endregion

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);

			this.solarSystemIndex = (int) ViewState["solarSystemIndex"];
			this.planetIndex = (int) ViewState["planetIndex"];
		}

		protected override object SaveViewState()
		{
			ViewState["solarSystemIndex"] = this.solarSystemIndex;
			ViewState["planetIndex"] = this.planetIndex;
			ViewState["moonIndex"] = this.moonIndex;

			return base.SaveViewState();
		}

		protected override void LoadControlState(object savedState)
		{
			this.cacheKey = (string) savedState;
		}

		protected override object SaveControlState()
		{
			return this.cacheKey;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			RegisterRequiresControlState(this);

			if (IsPostBack)
				return;

			this._starCluster = Global.Universe.ObjectFactory.StarClusters.Find(t => t.TemplateID == ulong.Parse(Request.QueryString["t"])).Clone();
			this.cacheKey = Guid.NewGuid().ToString();
			Cache.Add(this.cacheKey, this._starCluster, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), CacheItemPriority.High, null);
		}

		public override void DataBind()
		{
			base.DataBind();

			this.chkAutoName.Checked = (StarCluster.Name == null);
			this.txtName.Text = StarCluster.Name;
			this.txtX.Text = StarCluster.Coordinates.X.ToString("n4");
			this.txtY.Text = StarCluster.Coordinates.Y.ToString("n4");

			if ( this.chkAutoName.Checked )
				this.txtName.Style[HtmlTextWriterStyle.Display] = "none";
		}

		protected void OnAddSolarSystem(object sender, EventArgs e)
		{
			var solarSystem = new SolarSystem.Template(Global.Universe.ObjectFactory, StarCluster);

			this.solarSystemIndex = StarCluster.SolarSystems.Count;
			StarCluster.SolarSystems.Add(solarSystem);

			this._planets.DataBind();
			this._planets.Update();
			this._solarsystems.DataBind();
		}

		protected void OnAddPlanet(object sender, EventArgs e)
		{
			var planet = new Planet.Template(Global.Universe.ObjectFactory, SolarSystem);
			
			this.planetIndex = SolarSystem.Planets.Count;			
			SolarSystem.Planets.Add(planet);

			this._planets.DataBind();
			this._moons.DataBind();
			this._moons.Update();
		}

		protected void OnAddMoon(object sender, EventArgs e)
		{
			var moon = new Moon.Template(Global.Universe.ObjectFactory, Planet);
			Planet.Moons.Add(moon);

			this._moons.DataBind();
		}

		protected void OnMoonCommand(object source, RepeaterCommandEventArgs e)
		{
			switch ( e.CommandName )
			{
				case "edit":
					this.moonIndex = e.Item.ItemIndex;
					var moon = FindFooterControl<MoonDetails>((Repeater)source, "solarsystem");
					moon.Show();
					break;

				case "delete":
					Planet.Moons.RemoveAt(e.Item.ItemIndex);
					this._moons.DataBind();
					break;

				default:
					this._moons.DataBind();
					break;
			}
		}

		protected void OnPlanetCommand(object source, RepeaterCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "edit":
					this.planetIndex = e.Item.ItemIndex;
					var planet = FindFooterControl<PlanetDetails>((Repeater)source, "solarsystem");
					planet.Show();
					break;

				case "moons":
					this.planetIndex = e.Item.ItemIndex;
					this._moons.DataBind();
					this._moons.Update();
					break;

				case "delete":
					StarCluster.SolarSystems.RemoveAt(e.Item.ItemIndex);
					this._planets.DataBind();
					break;

				default:
					this._planets.DataBind();
					break;
			}
		}

		protected void OnSolarSystemCommand(object source, RepeaterCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "edit":
					var solarsystem = FindFooterControl<SolarSystemDetails>((Repeater) source, "solarsystem");
					solarsystem.Show();
					break;

				case "planets":
					this.solarSystemIndex = e.Item.ItemIndex;
					this._planets.DataBind();
					this._planets.Update();
					break;

				case "delete":
					StarCluster.SolarSystems.RemoveAt(e.Item.ItemIndex);
					this._solarsystems.DataBind();
					break;

				default:
					this._solarsystems.DataBind();
					break;
			}
		}

		private static T FindFooterControl<T>(Repeater repeater, string id) where T : Control
		{
			Control footer = repeater.Controls[repeater.Controls.Count - 1];
			return (T) footer.FindControl(id);
		}

		protected void OnUpdate(object sender, EventArgs e)
		{
			foreach (StarCluster.Template cluster in Global.Universe.ObjectFactory.StarClusters)
			{
				if (cluster.TemplateID == StarCluster.TemplateID)
					continue;

				if (Vector.Intersects(StarCluster.Coordinates, StarCluster.Extent, cluster.Coordinates, cluster.Extent) == false)
					continue;

				this.badPosition.IsValid = false;
				this.badPosition.ErrorMessage = string.Format("Sorry, but this cluster will collide with the {0} cluster", cluster.Name);
				return;
			}

			var actual = Global.Universe.ObjectFactory.GetTemplate<StarCluster.Template>(StarCluster.TemplateID);
			int index = Global.Universe.ObjectFactory.StarClusters.IndexOf(actual);
			Global.Universe.ObjectFactory.StarClusters[index] = actual;
			Global.SaveTemplates();

			Response.Redirect("~/admin/starclusters.aspx");
		}
	}
}