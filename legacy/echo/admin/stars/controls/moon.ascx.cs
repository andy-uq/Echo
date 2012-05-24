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
	public partial class MoonDetails : UserControl
	{
		public event EventHandler Updated;

		public Moon.Template Moon
		{
			get
			{
				return ((IStarClusterPage)Page).Moon;
			}
		}

		public override void DataBind()
		{
			if (Moon == null)
				return;

			base.DataBind();

			txtName.Text = Moon.Name;
			chkAutoName.Checked = (Moon.Name == null);
			txtX.Text = Moon.Coordinates.X.ToString("n4");
			txtY.Text = Moon.Coordinates.Y.ToString("n4");
		}

		protected void OnUpdate(object sender, CommandEventArgs e)
		{
			Moon.Name = chkAutoName.Checked ? null : txtName.Text;
			Moon.Size = double.Parse(txtSize.Text)*Units.MoonRadius*2d;
			Moon.Coordinates = new Vector(double.Parse(txtX.Text), double.Parse(txtY.Text), 0d);

			if ( Updated != null )
				Updated(this, EventArgs.Empty);

			_moon.Close();
		}

		public void Show()
		{
			DataBind();
			_moon.Show();
		}
	}
}