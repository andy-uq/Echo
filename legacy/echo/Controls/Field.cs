using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Echo.Web.Controls
{
	[PersistChildren(true), ParseChildren(false)]
	public class Field : WebControl
	{
		private static int autoNumberID;
		protected string label;
		protected string labelClass;

		private string primaryControlID;

		private string symbolicName;

		public Field()
			: base(HtmlTextWriterTag.Div)
		{
		}

		public string LabelClass
		{
			get { return labelClass; }
			set { labelClass = value; }
		}

		public string Label
		{
			get { return label; }
			set { label = value; }
		}

		[IDReferenceProperty]
		public string Control
		{
			get { return primaryControlID; }
			set { primaryControlID = value; }
		}

		public override string ID
		{
			get { return base.ID ?? GenerateAutoID(); }
			set { base.ID = value; }
		}

		private static string GenerateAutoID()
		{
			return string.Format("lbl{0}", autoNumberID++);
		}

		protected virtual string ValueID
		{
			get
			{
				string valueID = null;

				if (label != null || symbolicName != null)
				{
					Control primaryControl = null;
					if (primaryControlID != null)
						primaryControl = FindControl(primaryControlID);

					if (primaryControl != null)
						valueID = primaryControl.ClientID;
				}
				return valueID;
			}
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);

			label = (string) ViewState["label"];
			symbolicName = (string)ViewState["symbolicName"];
		}

		protected override object SaveViewState()
		{
			ViewState["label"] = label;
			ViewState["symbolicName"] = symbolicName;

			return base.SaveViewState();
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (string.IsNullOrEmpty(CssClass))
				CssClass = "inline";

			base.OnPreRender(e);
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			// Enclose label tag with extra div to keep width
			writer.AddAttribute("class", "help-label-container");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// Open label
			writer.AddAttribute("class", labelClass ?? "field");

			if (ValueID != null)
				writer.AddAttribute("for", ValueID);
			
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(label);

			// Close label
			writer.RenderEndTag();

			// Close enclosing div
			writer.RenderEndTag();

			base.RenderContents(writer);
		}
	}
}