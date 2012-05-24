using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Web.Controls.Base;

namespace Echo.Web
{
	public partial class Site : EchoMasterPage
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			ScriptManager.AsyncPostBackError += OnAsyncPostBackError;
		}

		private void OnAsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
		{
			ScriptManager.AllowCustomErrorsRedirect = false;
			ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message;
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			
			head.Title = (SubTitle == null) ? (Title ?? "Echo") : string.Concat(Title ?? "Echo", " - ", SubTitle);
			phTitle.Visible = (SubTitle != string.Empty);
			litSubTitle.Text = SubTitle ?? Title;
            
			string startupScript = "$(document).ready(function() { isReady(); });";
			ScriptManager.RegisterClientScriptBlock(Page, typeof(Site), "startup", startupScript, true);
		}
	}
}
