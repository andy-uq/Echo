using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Objects;
using Echo.Vectors;
using Echo.Web.Controls;
using Echo.Web.Controls.Base;

namespace Echo.Web.Administration.Stars
{
	public partial class SolarSystemDetails : System.Web.UI.UserControl
	{
		private Popup popup;

		public DisplayMode Display { get; set; }

		public event EventHandler Updated;

		public SolarSystemDetails()
		{
			this.popup = new Popup();
			this.popup.ID = "solarsystem";
		}

		public SolarSystem.Template SolarSystem
		{
			get
			{
				return ((IStarClusterPage)Page).SolarSystem;
			}
		}

		public override ControlCollection Controls
		{
			get
			{
				if ( Display == DisplayMode.Inline )
					return base.Controls;
				
				return popup.Controls;
			}
		}

		protected override void CreateChildControls()
		{
			if (Display == DisplayMode.Inline)
			{
				base.CreateChildControls();
			}
			else
			{
				base.CreateChildControls();
				
				foreach (Control ctrl in base.Controls)
					popup.Controls.Add(ctrl);

				base.Controls.Clear();
				base.Controls.Add(popup);
			}
		}

		public override void DataBind()
		{
			if (SolarSystem == null)
				return;

			base.DataBind();

			txtName.Text = SolarSystem.Name;
			chkAutoName.Checked = (SolarSystem.Name == null);
			txtX.Text = SolarSystem.Coordinates.X.ToString("n4");
			txtY.Text = SolarSystem.Coordinates.Y.ToString("n4");
		}

		public void Update()
		{
			SolarSystem.Name = chkAutoName.Checked ? null : txtName.Text;
			SolarSystem.Coordinates = new Vector(double.Parse(txtX.Text), double.Parse(txtY.Text), 0d);			
		}

		protected void OnUpdate(object sender, CommandEventArgs e)
		{
			if ( Updated != null )
				Updated(this, EventArgs.Empty);

			popup.Close();
		}

		public void Show()
		{
			DataBind();
			popup.Show();
		}
	}
}