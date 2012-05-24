using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Echo.Web.Controls
{
	[PersistChildren(true), ParseChildren(false)]
	public class Panel : WebControl, INamingContainer
	{
		public override string UniqueID
		{
			get { return ID; }
		}

		public override string ClientID
		{
			get { return ID; }
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}
	}
}