using System;
using System.Web;
using System.Web.UI;

using Ubiquity.u2ool;

namespace Echo.Web.Controls
{
	public delegate string RenderValueEventHandler(object value);

	public class FieldValue : Field
	{
		private string nullValue;
		private string outputFormat;
		private object value;
		private string valueClass;
		private bool hideIfNull;
		private bool encodeValue = true;

		public object Value
		{
			get { return value; }
			set { this.value = value; }
		}

		public string OutputFormat
		{
			get { return outputFormat; }
			set { outputFormat = value; }
		}

		public string ValueClass
		{
			get { return valueClass; }
			set { valueClass = value; }
		}

		public string NullValue
		{
			get { return nullValue; }
			set { nullValue = value; }
		}

		public bool HideIfNull
		{
			get { return hideIfNull; }
			set { hideIfNull = value; }
		}

		public bool EncodeValue
		{
			get { return this.encodeValue; }
			set { this.encodeValue = value; }
		}

		protected override string ValueID
		{
			get { return string.Format("{0}_value", ClientID); }
		}

		public event RenderValueEventHandler RenderValue;

		protected override void OnPreRender(EventArgs e)
		{
			if (string.IsNullOrEmpty(CssClass))
			{
				CssClass = "inline";
			}

			base.OnPreRender(e);

			if (hideIfNull && value == null)
			{
				Visible = false;
			}
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			base.RenderContents(writer);

			writer.BeginRender();
			writer.AddAttribute("class", valueClass ?? "value");
			writer.AddAttribute("id", ValueID);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			object renderedValue = value;
			if (RenderValue != null)
			{
				renderedValue = RenderValue(value);
			}

			if (renderedValue != null)
			{
				if (outputFormat == null)
				{
					if (renderedValue is Enum)
					{
						renderedValue = EnumHelper.GetText((Enum)renderedValue);
					}
					else
					{
						renderedValue = renderedValue.ToString();
					}
				}
				else
				{
					renderedValue = string.Format(outputFormat, renderedValue);
				}
			}
			else
			{
				renderedValue = nullValue;
			}

			if (renderedValue != null)
			{
				if (this.encodeValue)
				{
					writer.WriteEncodedText((string )renderedValue);
				}
				else
				{
					writer.Write(renderedValue);
				}
			}

			writer.RenderEndTag();
			writer.EndRender();
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			value = ViewState["value"];
		}

		protected override object SaveViewState()
		{
			ViewState["value"] = value;
			return base.SaveViewState();
		}
	}
}