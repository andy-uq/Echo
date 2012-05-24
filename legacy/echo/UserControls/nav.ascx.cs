using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Web.Controls.Base;

namespace Echo.Web.Controls
{
	public partial class MenuNavControl : UserControl
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Visible = Request.IsAuthenticated && (Page is LogoutPage) == false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "nav");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			writer.RenderBeginTag(HtmlTextWriterTag.Ul);

			if ( Page is EchoAdminPage )
			{
				WriteItem(writer, "Dashboard", "/admin/default.aspx", Page is IDashboardPage ? "left active" : "left");
				WriteItem(writer, "Star Clusters", "/admin/starclusters.aspx", Page is IStarClusterPage ? "active" : "");
				WriteItem(writer, "Ships", "/admin/ships.aspx", Page is IShipsPage ? "active" : "");
				WriteItem(writer, "Agents", "/admin/personnel.aspx", Page is IPersonnelPage ? "active" : "");
				WriteItem(writer, "Structures", "/admin/structures.aspx", Page is IStructuresPage ? "right active" : "right");
			}
			else
			{
				WriteItem(writer, "Dashboard", "/members/default.aspx", Page is IDashboardPage ? "left active" : "left");
				WriteItem(writer, "Market", "/members/market.aspx", Page is IMarketPage ? "active" : "");
				WriteItem(writer, "Ships", "/members/ships.aspx", Page is IShipsPage ? "active" : "");
				WriteItem(writer, "Personnel", "/members/personnel.aspx", Page is IPersonnelPage ? "active" : "");
				WriteItem(writer, "Structures", "/members/structures.aspx", Page is IStructuresPage ? "right active" : "right");
			}

			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		private static void WriteItem(HtmlTextWriter writer, string text, string href, string cssClass)
		{
			if (string.IsNullOrEmpty(cssClass) == false)
				writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);

			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			writer.AddAttribute(HtmlTextWriterAttribute.Href, href);
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write(text);
			writer.RenderEndTag();
			writer.RenderEndTag();
		}
	}
}