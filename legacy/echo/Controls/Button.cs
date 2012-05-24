using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Echo.Web.Controls
{
	public class Button : LinkButton
	{
		public string NavigateUrl { get; set; }

		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, "-container"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "button");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);

			if ( NavigateUrl == null )
			{
				AddAttributesToRender(writer);
			}
			else
			{
				string path = NavigateUrl;
				string qs = string.Empty;
				int splitIndex = path.IndexOf('?');

				if ( splitIndex != -1 )
				{
					path = NavigateUrl.Substring(0, splitIndex);
					qs = NavigateUrl.Substring(splitIndex);
				}
                
				path = VirtualPathUtility.ToAbsolute(path);                
				writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Concat(path, qs));
			}

			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.WriteEncodedText(Text);
			writer.RenderEndTag();

			writer.RenderEndTag();
		}
	}
}