using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Maths;
using Echo.Objects;
using Echo.Vectors;
using Echo.Web.Controls.Base;

namespace Echo.Web.Administration.Stars
{
	public partial class PlanetDetails : UserControl
	{
		public event EventHandler Updated;
		
		public Planet.Template Planet
		{
			get
			{
				return ((IStarClusterPage)Page).Planet;
			}
		}

		public override void DataBind()
		{
			if (Planet == null)
				return;

			base.DataBind();

			txtName.Text = Planet.Name;
			chkAutoName.Checked = (Planet.Name == null);
			txtSize.Text = Planet.Size.ToString("n4");
			txtX.Text = Planet.Coordinates.X.ToString("n4");
			txtY.Text = Planet.Coordinates.Y.ToString("n4");
		}

		public void Update()
		{
			Planet.Name = this.chkAutoName.Checked ? null : this.txtName.Text;
			Planet.Size = double.Parse(this.txtSize.Text)*Units.EarthRadius*2d;
			Planet.Coordinates = new Vector(double.Parse(this.txtX.Text), double.Parse(this.txtY.Text), 0d);
		}

		protected void OnUpdate(object sender, CommandEventArgs e)
		{
			Update();

			if ( Updated != null )
				Updated(this, EventArgs.Empty);

			_planet.Close();
		}


		public void Show()
		{
			DataBind();
			_planet.Show();
		}
	}
}