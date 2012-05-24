using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Echo.Web.Controls
{
	public partial class LoggedInControl : System.Web.UI.UserControl
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if ( Page is LogoutPage )
			{
				phLoggedIn.Visible = false;
				phNotLoggedIn.Visible = true;
			}
			else
			{
				litUserName.Text = Page.User.Identity.Name;
				phLoggedIn.Visible = Request.IsAuthenticated;
				phNotLoggedIn.Visible = (Request.IsAuthenticated == false);
			}
		}
	}
}