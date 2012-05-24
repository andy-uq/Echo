using System.Web.UI;

namespace Echo.Web.Controls
{
	public class ClientButton : Control
	{
		public string Text { get; set; }
		public string Action { get; set; }

		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, "-container"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "button");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);

			writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Concat("javascript:", Action));
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.WriteEncodedText(Text);
			writer.RenderEndTag();

			writer.RenderEndTag();
		}
	}
}