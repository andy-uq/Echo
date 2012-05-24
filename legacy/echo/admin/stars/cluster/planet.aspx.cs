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
	public partial class EditPlanetPage : EchoAdminPage, IStarClusterPage
	{
		private StarCluster.Template _starCluster;
		private string cacheKey;
		private int moonIndex = -1;
		private int planetIndex = -1;
		private int solarSystemIndex = -1;

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

			return base.SaveViewState();
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

		protected override void LoadControlState(object savedState)
		{
			this.cacheKey = (string) savedState;
		}

		protected override object SaveControlState()
		{
			return this.cacheKey;
		}

		protected void OnAddSolarSystem(object sender, EventArgs e)
		{
			var solarSystem = new SolarSystem.Template(Global.Universe.ObjectFactory, StarCluster);

			this.solarSystemIndex = StarCluster.SolarSystems.Count;
			StarCluster.SolarSystems.Add(solarSystem);

			this._planets.DataBind();
			this._planets.Update();
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
					var moon = FindFooterControl<MoonDetails>((Repeater)source);
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
					var planet = FindFooterControl<PlanetDetails>((Repeater) source);
					planet.Show();
					break;

				case "moons":
					this.planetIndex = e.Item.ItemIndex;
					this._moons.DataBind();
					this._moons.Update();
					break;

				case "delete":
					Planets.RemoveAt(e.Item.ItemIndex);
					this._planets.DataBind();
					break;

				default:
					this._planets.DataBind();
					break;
			}
		}

		private static T FindFooterControl<T>(Repeater repeater) where T : Control
		{
			Control footer = repeater.Controls[repeater.Controls.Count - 1];
			return (T) footer.FindControl("solarsystem");
		}

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