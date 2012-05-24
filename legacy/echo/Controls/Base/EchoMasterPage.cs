using System.Web.UI;

namespace Echo.Web.Controls.Base
{
	public class EchoMasterPage : MasterPage
	{
		protected ScriptManager _scriptmanager;

		public EchoPage EchoPage
		{
			get { return (EchoPage)Page; }
		}

		public string Title
		{
			get { return string.IsNullOrEmpty(EchoPage.Title) ? "Echo" : EchoPage.Title; }
		}

		public string SubTitle
		{
			get { return EchoPage.SubTitle; }
		}

		public ScriptManager ScriptManager
		{
			get { return this._scriptmanager; }
		}
	}
}