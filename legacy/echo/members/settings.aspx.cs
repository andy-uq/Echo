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
	public partial class MySettingsPage : EchoPage, IDashboardPage
	{
		protected void OnChangePassword(object sender, EventArgs e)
		{
			try
			{
				var user = Membership.GetUser(Page.User.Identity.Name);
				if (user.ChangePassword(txtOldPassword.Text, txtPassword.Text))
				{
					changepwdConfirm.Show();
				}
				else
				{
					operation.IsValid = false;
					operation.ErrorMessage = "Sorry, but your old password is incorrect.";
				}
			}
			catch (Exception ex)
			{
				operation.IsValid = false;
				operation.ErrorMessage = ex.Message;
			}
		}
	}
}
