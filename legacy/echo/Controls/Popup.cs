using System.Web.UI;

namespace Echo.Web.Controls
{
	[ParseChildren(false), PersistChildren(false)]
	public class Popup : Control
	{
		private bool show;

		public override string ClientID
		{
			get
			{
				return ID;
			}
		}

		public override string UniqueID
		{
			get
			{
				return ID;
			}
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			show = (bool) ViewState["show"];
		}

		protected override object SaveViewState()
		{
			ViewState["show"] = show;
			return base.SaveViewState();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "popup");
			
			if (show == false)
			writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");

			// open outer DIV
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// alpha
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "alpha");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.RenderEndTag();

			// open container
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, "-inner"));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "inner");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// header
			if ( string.IsNullOrEmpty(Title) == false )
			{
				writer.RenderBeginTag(HtmlTextWriterTag.H3);
				writer.WriteEncodedText(Title);
				writer.RenderEndTag();
			}

			base.RenderChildren(writer);

			writer.AddAttribute(HtmlTextWriterAttribute.Class, "button close");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.AddAttribute(HtmlTextWriterAttribute.Rel, string.Concat("#", ClientID));
			//writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:while(0)");
			//writer.AddAttribute(HtmlTextWriterAttribute.Title, "Close");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.WriteEncodedText("Close");
			writer.RenderEndTag();
			writer.RenderEndTag();

			// close container
			writer.RenderEndTag();

			// close outer DIV
			writer.RenderEndTag();
		}

		public string Title { get; set; }

		public void Show()
		{
			this.show = true;
		}

		public void Close()
		{
			this.show = false;
		}
	}
}