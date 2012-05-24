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
	public partial class RegisterPage : EchoPage
	{
		protected void OnRegister(object sender, EventArgs e)
		{
			MembershipCreateStatus status;
			
			var member = Membership.CreateUser(this.txtUsername.Text, this.txtPassword.Text, this.txtEmailAddress.Text, null, null, false, out status);
			if (status != MembershipCreateStatus.Success)
			{
				operation.IsValid = false;
				operation.ErrorMessage = ErrorCodeToString(status);

				return;
			}

			Global.Universe.RegisterPlayerCorporation(this.txtCorporationName.Text, member.UserName, member.Email);
			member.ChangePassword(txtPassword.Text, txtPassword.Text);

			Global.SaveState();

			FormsAuthentication.SetAuthCookie(this.txtUsername.Text, false);
			Response.Redirect("~/default.aspx");
		}
	
		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
			// a full list of status codes.
			switch ( createStatus )
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Username already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A username for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
	}
}
