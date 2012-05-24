using System.Web.UI.HtmlControls;

namespace Echo.Web.Controls.Base
{
	public class EchoAdminPage : EchoPage
	{
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			var adminCss = new HtmlLink {Href = "~/css/admin.css",};
			adminCss.Attributes["rel"] = "stylesheet";
			adminCss.Attributes["type"] = "text/css";

			Header.Controls.Add(adminCss);
		}
	}
}