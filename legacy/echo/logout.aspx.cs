using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Web.Controls.Base;

namespace Echo.Web
{
	public partial class LogoutPage : EchoPage
	{
		protected override void OnInit(EventArgs e)
		{
			FormsAuthentication.SignOut();
			base.OnInit(e);
		}
	}
}
