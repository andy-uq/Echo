using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Principal;

using Echo.Web.Controls.Base;

namespace Echo.Web
{
	public partial class LoginPage : EchoPage
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			EchoMasterPage.ScriptManager.RegisterAsyncPostBackControl(btnLogin);
		}

		protected void OnSubmit(object sender, EventArgs e)
		{
			if (Membership.ValidateUser(txtUsername.Text, txtPassword.Text))
			{
				FormsAuthentication.SetAuthCookie(txtUsername.Text, chkRememberMe.Checked);
				var returnUrl = Request.QueryString["returnUrl"];
				if (string.IsNullOrEmpty(returnUrl))
				{
					returnUrl = "~/members/default.aspx";
				}

				Response.Redirect(returnUrl);
			}
			else
			{
				operation.IsValid = false;
				operation.ErrorMessage = "Incorrect username or password.  Please try again.";
				validation.Update();
			}
		}
	}
}
