using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ubiquity.u2ool.Collections;

namespace Echo.Web.Controls
{
	public class Validation : ValidationSummary
	{
		public override string ID
		{
			get { return "error"; }
			set { }
		}

		public override string ClientID
		{
			get { return ID; }
		}

		public override string UniqueID
		{
			get { return ID; }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			EnableClientScript = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);

			var invalid = new List<IValidator>();
			foreach (IValidator v in Page.Validators)
			if (v.IsValid == false)
				invalid.Add(v);

			if (invalid.Count == 0)
				writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");

			// open outer DIV
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// alpha
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "alpha");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.RenderEndTag();

			// open container
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, "-inner"));
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// header
			if (string.IsNullOrEmpty(HeaderText) == false)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Strong);
				writer.WriteEncodedText(HeaderText);
				writer.RenderEndTag();
			}

			// open inner UL
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);

			if (Page.IsPostBack)
			{
				foreach (IValidator validator in Page.Validators)
				{
					if (validator.IsValid)
						continue;

					writer.RenderBeginTag(HtmlTextWriterTag.Li);
					writer.WriteEncodedText(validator.ErrorMessage);
					writer.RenderEndTag();
				}
			}

			// close UL
			writer.RenderEndTag();

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
	}
}